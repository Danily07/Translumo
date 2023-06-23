using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using OpenCvSharp;
using Translumo.Infrastructure;
using Translumo.OCR;
using Translumo.OCR.Configuration;
using Translumo.Processing.Exceptions;
using Translumo.Processing.Interfaces;
using Translumo.Processing.TextProcessing;
using Translumo.Translation;
using Translumo.Translation.Configuration;
using Translumo.Translation.Exceptions;

namespace Translumo.Processing
{

    public class TranslationProcessingService : IProcessingService
    {
        public bool IsStarted => !_ctSource?.IsCancellationRequested ?? false;

        private readonly ICapturerFactory _capturerFactory;
        private readonly IChatTextMediator _chatTextMediator;
        private readonly OcrEnginesFactory _enginesFactory;
        private readonly TranslatorFactory _translatorFactory;
        private readonly TextDetectionProvider _textProvider;
        private readonly TextResultCacheService _textResultCacheService;
        private readonly ILogger _logger;
        private readonly AutoResetEvent _translationSync = new AutoResetEvent(true);

        private IEnumerable<IOCREngine> _engines;
        private ITranslator _translator;
        private TranslationConfiguration _translationConfiguration;
        private OcrGeneralConfiguration _ocrGeneralConfiguration;

        private CancellationTokenSource _ctSource;

        private const float MIN_SCORE_THRESHOLD = 2.1f;
        
        public TranslationProcessingService(ICapturerFactory capturerFactory, IChatTextMediator chatTextMediator, OcrEnginesFactory ocrEnginesFactory, TranslatorFactory translationFactory,
            TextDetectionProvider textProvider, TranslationConfiguration translationConfiguration, OcrGeneralConfiguration ocrConfiguration, 
            TextResultCacheService textResultCacheService, ILogger<TranslationProcessingService> logger)
        {
            _logger = logger;
            _chatTextMediator = chatTextMediator;
            _capturerFactory = capturerFactory;
            _translationConfiguration = translationConfiguration;
            _ocrGeneralConfiguration = ocrConfiguration;
            _enginesFactory = ocrEnginesFactory;
            _textProvider = textProvider;
            _textResultCacheService = textResultCacheService;
            _translatorFactory = translationFactory;

            _engines = InitializeEngines();
            _translator = _translatorFactory.CreateTranslator(_translationConfiguration);
            _textProvider.Language = translationConfiguration.TranslateFromLang;

            _translationConfiguration.PropertyChanged += TranslationConfigurationOnPropertyChanged;
            _ocrGeneralConfiguration.PropertyChanged += OcrGeneralConfigurationOnPropertyChanged;
        }

        public void StartProcessing()
        {
            if (IsStarted)
            {
                return;
            }

            if (!_engines.Any())
            {
                _chatTextMediator.SendText("No OCR engine is selected!", false);
                return;
            }

            _ctSource = new CancellationTokenSource();
            Task.Factory.StartNew(() => TranslateInternal(_ctSource.Token));

            _chatTextMediator.SendText("Translation started", TextTypes.Info);
        }

        public void StopProcessing()
        {
            _ctSource.Cancel();

            _chatTextMediator.SendText("Translation finished", TextTypes.Info);
        }

        private void TranslateInternal(CancellationToken cancellationToken)
        {
            _translationSync.WaitOne();

            const int MAX_TRANSLATE_TASK_POOL = 4;
            const int SHORT_ITERATION_DELAY_MS = 130;
            const int ITERATION_DELAY_MS = 330;
            const int SEQUENTIAL_DIFF_LETTERS = 3;

            IOCREngine primaryOcr = _engines.OrderByDescending(e => e.PrimaryPriority).First();
            IOCREngine[] otherOcr = _engines.Except(new[] { primaryOcr }).ToArray();

            var detectedResults = new Task<TextDetectionResult>[otherOcr.Length + 1];
            var activeTranslationTasks = new List<Task>();
            Mat cachedImg = null;
            Guid iterationId;
            bool? lastIterationFull = null;
            bool sequentialText = false;

            TextDetectionResult GetSecondaryCheckText(byte[] screen)
            {
                Mat grayScaleScreen = ImageHelper.ToGrayScale(screen);
                if (cachedImg != null)
                {
                    var unitedScreen = ImageHelper.UnionImages(cachedImg, grayScaleScreen);

                    cachedImg?.Dispose();
                    cachedImg = grayScaleScreen;

                    return _textProvider.GetText(primaryOcr, unitedScreen);
                }

                cachedImg = grayScaleScreen;

                return null;
            }

            using var capturer = _capturerFactory.CreateCapturer();
            if (capturer == null)
            {
                _chatTextMediator.SendText("Failed to initialize capturer. Please check logs for details", false);
                _translationSync.Set();

                return;
            }

            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    if (lastIterationFull.HasValue)
                    {
                        Thread.Sleep(lastIterationFull.Value ? SHORT_ITERATION_DELAY_MS : ITERATION_DELAY_MS);
                        _textResultCacheService.EndIteration();
                    }

                    var faultedTask = activeTranslationTasks.FirstOrDefault(t => t.IsFaulted);
                    activeTranslationTasks.RemoveAll(task => task.IsCompleted);
                    if (faultedTask != null)
                    {
                        throw faultedTask.Exception.InnerException;
                    }

                    if (activeTranslationTasks.Count >= MAX_TRANSLATE_TASK_POOL)
                    {
                        continue;
                    }

                    byte[] screenshot = capturer.CaptureScreen();
                    lastIterationFull = false;
                    var primaryDetected = _textProvider.GetText(primaryOcr, screenshot);
                    if (primaryDetected.ValidityScore == 0 || _textResultCacheService.IsCached(primaryDetected.Text, sequentialText))
                    {
                        continue;
                    }

                    if (primaryOcr.SecondaryPrimaryCheck)
                    {
                        var res = GetSecondaryCheckText(screenshot);
                        if (res != null && _textResultCacheService.IsCached(res.Text, false))
                        {
                            if (primaryDetected.Text.Length - res.Text.Length > SEQUENTIAL_DIFF_LETTERS)
                            {
                                sequentialText = true;
                            }
                            
                            continue;
                        }
                    }

                    for (var i = 0; i < otherOcr.Length; i++)
                    {
                        detectedResults[i] = _textProvider.GetTextAsync(otherOcr[i], screenshot);
                    }

                    detectedResults[^1] = Task.FromResult(primaryDetected);
                    Task.WaitAll(detectedResults);

                    TextDetectionResult bestDetected = GetBestDetectionResult(detectedResults, 3);
                    if (bestDetected.ValidityScore <= MIN_SCORE_THRESHOLD)
                    {
                        sequentialText = false;
                        continue;
                    }

                    lastIterationFull = true;
                    if (_textResultCacheService.IsCached(bestDetected.Text, bestDetected.ValidityScore, sequentialText, out iterationId))
                    {
                        sequentialText = false;
                        continue;
                    }

                    sequentialText = false;
                    //resultLogger.LogResults(detectedResults.Select(res => res.Result), screenshot);
                    activeTranslationTasks.Add(TranslateTextAsync(bestDetected.Text, iterationId));
                }
                catch (CaptureException ex)
                {
                    if (!lastIterationFull.HasValue)
                    {
                        _chatTextMediator.SendText($"Failed to capture screen ({ex.Message})", false);
                        lastIterationFull = true;
                    }

                    _logger.LogError(ex, $"Screen capture failed (code: {ex.ErrorCode})");
                    capturer.Initialize();
                }
                catch (TranslationException)
                {
                    _chatTextMediator.SendText("Text translation is failed", false);
                }
                catch (AggregateException ex) when (ex.InnerException is TextDetectionException innerEx)
                {
                    _chatTextMediator.SendText($"Text detection is failed ({innerEx.SourceOCREngineType.Name})", false);
                    _logger.LogError(ex, $"Unexpected error during text detection ({innerEx.SourceOCREngineType})");
                }
                catch (Exception ex)
                {
                    _chatTextMediator.SendText($"Unknown error: {ex.Message}", false);
                    _logger.LogError(ex, $"Processing iteration failed due to unknown error");
                }
            }
            _textResultCacheService.Reset();
            _logger.LogTrace("Translation finished");
            _translationSync.Set();
        }

        private async Task TranslateTextAsync(string text, Guid iterationId)
        {
            var translation = await _translator.TranslateTextAsync(text);
            if (!string.IsNullOrWhiteSpace(translation) && !_textResultCacheService.IsTranslatedCached(translation, iterationId))
            {
                _chatTextMediator.SendText(translation, true);
            }
        }

        private TextDetectionResult GetBestDetectionResult(Task<TextDetectionResult>[] results, int minCountSameResults)
        {
            var maxScoreIndex = 0;
            for (var i = 0; i < results.Length; i++)
            {
                maxScoreIndex = results[maxScoreIndex].Result.CompareTo(results[i].Result) > 0 ? maxScoreIndex : i;
                if (i > results.Length - minCountSameResults || results[i].Result.ValidityScore == 0)
                {
                    continue;
                }

                var intRowCount = 1;
                var inRowIndex = i;
                for (var j = i + 1; j < results.Length; j++)
                {
                    if (results[i].Result.ValidatedText == results[j].Result.ValidatedText)
                    {
                        intRowCount++;
                        inRowIndex = results[inRowIndex].Result.SourceEngine.Confidence > results[j].Result.SourceEngine.Confidence
                            ? inRowIndex
                            : j;
                    }
                }
                //If array contains multiple (=minCountSameResults) same results, consider it as the best
                if (intRowCount >= minCountSameResults)
                {
                    results[inRowIndex].Result.ValidityScore = float.MaxValue;
                    return results[inRowIndex].Result;
                }
            }

            return results[maxScoreIndex].Result;
        }

        private void TranslationConfigurationOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_translationConfiguration.TranslateFromLang))
            {
                _engines = InitializeEngines();
                _textProvider.Language = _translationConfiguration.TranslateFromLang;
            }

            _translator = _translatorFactory.CreateTranslator(_translationConfiguration);
        }

        private void OcrGeneralConfigurationOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            _engines = InitializeEngines();
        }

        private IEnumerable<IOCREngine> InitializeEngines()
        {
            return _enginesFactory
                .GetEngines(_ocrGeneralConfiguration.OcrConfigurations, _translationConfiguration.TranslateFromLang)
                .ToArray();
        }
    }
}
