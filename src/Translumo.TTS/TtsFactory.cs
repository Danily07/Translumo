using Microsoft.Extensions.Logging;
using Translumo.Infrastructure.Language;
using Translumo.Infrastructure.Python;
using Translumo.TTS.Engines;
using Translumo.Utils.Extensions;

namespace Translumo.TTS
{
    public class TtsFactory
    {
        private readonly LanguageService _languageService;
        private readonly PythonEngineWrapper _pythonEngine;
        private readonly ILogger _logger;
        private readonly TaskScheduler _uiScheduler;
        private CancellationTokenSource _cancelationTokenSource;

        public TtsFactory(LanguageService languageService, PythonEngineWrapper pythonEngine, ILogger<TtsFactory> logger)
        {
            _languageService = languageService;
            _pythonEngine = pythonEngine;
            _logger = logger;
            _uiScheduler = TaskScheduler.FromCurrentSynchronizationContext();
            _cancelationTokenSource = new CancellationTokenSource();
        }

        public ITTSEngine CreateTtsEngine(TtsConfiguration ttsConfiguration)
        {
            ITTSEngine ttsEngine = ttsConfiguration.TtsSystem switch
            {
                TTSEngines.None => new NoneTTSEngine(),
                TTSEngines.WindowsTTS => new WindowsTTSEngine(GetLangCode(ttsConfiguration)),
                TTSEngines.SileroTTS => new SileroTTSEngine(_pythonEngine, GetLangCode(ttsConfiguration)),
                _ => throw new NotSupportedException()
            };

            UpdateAvailableAndCurrentVoiceAsync(ttsConfiguration, ttsEngine).ConfigureAwait(false);
            return ttsEngine;
        }

        private string GetLangCode(TtsConfiguration ttsConfiguration) =>
            _languageService.GetLanguageDescriptor(ttsConfiguration.TtsLanguage).Code;

        private async Task UpdateAvailableAndCurrentVoiceAsync(TtsConfiguration ttsConfiguration, ITTSEngine ttsEngine)
        {
            var voices = ttsEngine.GetVoices();
            var currentVoice = voices.Contains(ttsConfiguration.CurrentVoice)
                ? ttsConfiguration.CurrentVoice
                : voices.First();

            _cancelationTokenSource.Cancel();
            _cancelationTokenSource = new CancellationTokenSource();

            await RunOnUIAsync(() =>
                {
                    ttsConfiguration.AvailableVoices.Clear();
                    voices.ForEach(ttsConfiguration.AvailableVoices.Add);
                }, _cancelationTokenSource.Token);

            ttsConfiguration.CurrentVoice = currentVoice;
        }

        private Task RunOnUIAsync(Action action, CancellationToken token)
        {
            var taskFactory = new TaskFactory(
                token,
                TaskCreationOptions.DenyChildAttach,
                TaskContinuationOptions.None,
                _uiScheduler);

            return taskFactory.StartNew(action);
        }
    }
}
