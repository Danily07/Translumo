using Translumo.Infrastructure.Language;
using Translumo.Utils;

namespace Translumo.TTS;


public class TtsGeneralConfiguration : BindableBase
{
    public static TtsGeneralConfiguration Default =>
        new TtsGeneralConfiguration()
        {   
            TtsLanguage = Languages.English,
            TtsSystem = TTSEngines.None
        };

    private TTSEngines _ttsSystem;
    private Languages _ttsLanguage;
    private List<Languages> _installedWinTtsLanguages;

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