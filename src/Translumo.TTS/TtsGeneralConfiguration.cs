using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Translumo.Infrastructure.Language;
using Translumo.Utils;
using Translumo.Utils.Extensions;
using Windows.ApplicationModel.VoiceCommands;

namespace Translumo.TTS;


public class TtsGeneralConfiguration : BindableBase
{
    //public static TtsGeneralConfiguration Default2 => new TtsGeneralConfiguration()
    //{
    //    TtsConfigurations = new TtsConfiguration[] { new NoneTtsConfiguration(), new WindowsTtsConfiguration() },
    //    InstalledWinTtsLanguages = new List<Languages>()
    //};

    //public TtsConfiguration[] TtsConfigurations
    //{
    //    get => _ttsConfigurations;
    //    set
    //    {
    //        if (value != _ttsConfigurations)
    //        {
    //            _ttsConfigurations?.ForEach(conf => conf.PropertyChanged -= OcrConfigurationOnPropertyChanged);
    //            value?.ForEach(conf => conf.PropertyChanged += OcrConfigurationOnPropertyChanged);
    //        }

    //        _ttsConfigurations = value;
    //    }
    //}

    //public event PropertyChangedEventHandler PropertyChanged;

    //private TtsConfiguration[] _ttsConfigurations;


    //public TConfiguration GetConfiguration<TConfiguration>()
    //    where TConfiguration : TtsConfiguration
    //{
    //    return (TConfiguration)TtsConfigurations.FirstOrDefault(conf => conf.GetType() == typeof(TConfiguration));
    //}


    //private void OcrConfigurationOnPropertyChanged(object sender, PropertyChangedEventArgs e)
    //{
    //    OnPropertyChanged(nameof(TtsConfigurations));
    //}

    //[NotifyPropertyChangedInvocator]
    //protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    //{
    //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    //}


    private List<Languages> _installedWinTtsLanguages;

    public List<Languages> InstalledWinTtsLanguages
    {
        get => _installedWinTtsLanguages;
        set
        {
            _installedWinTtsLanguages = value;
            //OnPropertyChanged();
            SetProperty(ref _installedWinTtsLanguages, value);
        }
    }

    public static TtsGeneralConfiguration Default =>
        new TtsGeneralConfiguration()
        {   
            TtsLanguage = Languages.English,
            TtsSystem = TTSEngines.None
        };

    private TTSEngines _ttsSystem;
    private Languages _ttsLanguage;

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
}

public class NoneTtsConfiguration : TtsConfiguration
{
}