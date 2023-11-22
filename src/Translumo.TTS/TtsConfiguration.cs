using System.Collections.ObjectModel;
using Translumo.Infrastructure.Language;
using Translumo.Utils;

namespace Translumo.TTS;


public class TtsConfiguration : BindableBase
{
    public static TtsConfiguration Default =>
        new TtsConfiguration()
        {
            TtsLanguage = Languages.English,
            TtsSystem = TTSEngines.None,
            InstalledWinTtsLanguages = new List<Languages>(),
            _currentVoice = string.Empty,
            _availableVoices = new(),
        };

    private TTSEngines _ttsSystem;
    private Languages _ttsLanguage;
    private List<Languages> _installedWinTtsLanguages;
    private string _currentVoice;
    private ObservableCollection<string> _availableVoices;

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

    public ObservableCollection<string> AvailableVoices
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
            if (!_availableVoices.Contains(value))
            {
                return;
            }

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