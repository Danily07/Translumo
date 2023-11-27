using Microsoft.Extensions.Logging;
using Translumo.Infrastructure.Language;
using Translumo.Infrastructure.Python;
using Translumo.TTS.Engines;

namespace Translumo.TTS
{
    public class TtsFactory
    {
        private readonly LanguageService _languageService;
        private readonly PythonEngineWrapper _pythonEngine;
        private readonly IObserverAvailableVoices _observerAvailableVoices;
        private readonly ILogger _logger;

        public TtsFactory(LanguageService languageService, PythonEngineWrapper pythonEngine, IObserverAvailableVoices observerAvailableVoices, ILogger<TtsFactory> logger)
        {
            _languageService = languageService;
            _pythonEngine = pythonEngine;
            _observerAvailableVoices = observerAvailableVoices;
            _logger = logger;
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
            _observerAvailableVoices.UpdateVoice(voices);

            return ttsEngine;
        }

        private string GetLangCode(TtsConfiguration ttsConfiguration) =>
            _languageService.GetLanguageDescriptor(ttsConfiguration.TtsLanguage).Code;
    }
}
