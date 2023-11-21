using Microsoft.Extensions.Logging;
using System.Management.Automation.Language;
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

        public TtsFactory(LanguageService languageService, PythonEngineWrapper pythonEngine, ILogger<TtsFactory> logger)
        {
            _languageService = languageService;
            _pythonEngine = pythonEngine;
            _logger = logger;
            _uiScheduler = TaskScheduler.FromCurrentSynchronizationContext();
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

            var voices = ttsEngine.GetVoices();
            var currentVoice = voices.Contains(ttsConfiguration.CurrentVoice)
                ? ttsConfiguration.CurrentVoice
                : voices.First();

            RunOnUI(() =>
            {
                ttsConfiguration.AvailableVoices.Clear();
                voices.ForEach(ttsConfiguration.AvailableVoices.Add);
            }).Wait();

            ttsConfiguration.CurrentVoice = currentVoice;
            ttsEngine.SetVoice(currentVoice);

            return ttsEngine;
        }

        private string GetLangCode(TtsConfiguration ttsConfiguration) =>
            _languageService.GetLanguageDescriptor(ttsConfiguration.TtsLanguage).Code;

        private Task RunOnUI(Action action)
        {
            var taskFactory = new TaskFactory(
                CancellationToken.None,
                TaskCreationOptions.DenyChildAttach,
                TaskContinuationOptions.None,
                _uiScheduler);

            return taskFactory.StartNew(action);
        }
    }
}
