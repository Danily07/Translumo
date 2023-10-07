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
        private readonly ILogger _logger;

        public TtsFactory(LanguageService languageService, PythonEngineWrapper pythonEngine, ILogger<TtsFactory> logger)
        {
            _languageService = languageService;
            _pythonEngine = pythonEngine;
            _logger = logger;
        }

        public ITTSEngine CreateTtsEngine(TtsConfiguration ttsConfiguration) =>
            ttsConfiguration.TtsSystem switch
            {
                TTSEngines.None => new NoneTTSEngine(),
                TTSEngines.WindowsTTS => new WindowsTTSEngine(
                    _languageService.GetLanguageDescriptor(ttsConfiguration.TtsLanguage).Code),
                TTSEngines.SileroTTS => new SileroTTSEngine(_pythonEngine),
                _ => throw new NotSupportedException()
            };
    }
}
