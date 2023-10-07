using Translumo.Infrastructure.Language;
using Translumo.Utils;
using Windows.Security.EnterpriseData;

namespace Translumo.TTS;


public class TtsConfiguration : BindableBase
{
    protected TtsConfiguration()
    {
        // for serialization
    }

    public TtsConfiguration(LanguageService languageService)
    {
        TtsLanguage = Languages.English;
        TtsSystem = TTSEngines.None;
        InstalledWinTtsLanguages = new List<Languages>();
        _languageService = languageService;
    }

    private TTSEngines _ttsSystem;
    private Languages _ttsLanguage;
    private List<Languages> _installedWinTtsLanguages;
    private readonly LanguageService _languageService;

    public bool IsLanguageSupportedInTtsEngine(TTSEngines engine, Languages lang) =>
        TtsFactory.IsLanguageSupported(engine, lang, _languageService);

    public TTSEngines TtsSystem
    {
        get => _ttsSystem;
        set
        {
            SetProperty(ref _ttsSystem, value);
        }
    }

    public Languages TtsLanguage
    {
        get => _ttsLanguage;
        set
        {
            SetProperty(ref _ttsLanguage, value);
        }
    }

    public List<Languages> InstalledWinTtsLanguages
    {
        get => _installedWinTtsLanguages;
        set
        {
            SetProperty(ref _installedWinTtsLanguages, value);
        }
    }
}