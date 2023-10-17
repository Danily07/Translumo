using Translumo.Infrastructure.Language;
using Translumo.Utils;
using Windows.Security.EnterpriseData;

namespace Translumo.TTS;


public class TtsConfiguration : BindableBase
{
    public static TtsConfiguration Default =>
        new TtsConfiguration()
        {
            TtsLanguage = Languages.English,
            TtsSystem = TTSEngines.None,
            InstalledWinTtsLanguages = new List<Languages>(),
            _availableVoices = new(),
            _currentVoice = string.Empty
        };

    private TTSEngines _ttsSystem;
    private Languages _ttsLanguage;
    private List<Languages> _installedWinTtsLanguages;
    private List<string> _availableVoices;
    private string _currentVoice;

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

    public List<string> AvailableVoices
    {
        get => _availableVoices;
        set
        {
            SetProperty(ref _availableVoices, value);
        }
    }

    public string CurrentVoice
    {
        get => _currentVoice;
        set
        {
            SetProperty(ref _currentVoice, value);
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