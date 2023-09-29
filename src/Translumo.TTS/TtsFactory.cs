using Microsoft.Extensions.Logging;
using Translumo.Infrastructure.Language;
using Translumo.TTS.Engines;

namespace Translumo.TTS
{
    public class TtsFactory
    {
        private readonly LanguageService _languageService;
        private readonly ILogger _logger;

        public TtsFactory(LanguageService languageService, ILogger<TtsFactory> logger)
        {
            _languageService = languageService;
            _logger = logger;
        }

        public ITTSEngine CreateTtsEngine(TtsConfiguration ttsConfiguration) =>
            ttsConfiguration.TtsSystem switch
            {
                TTSEngines.None => new NoneTTSEngine(),
                TTSEngines.WindowsTTS => new WindowsTTSEngine(
                    _languageService.GetLanguageDescriptor(ttsConfiguration.TtsLanguage).Code),
                _ => throw new NotSupportedException()
            };
    }
}
