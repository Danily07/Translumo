using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using Translumo.Utils;

namespace Translumo.TTS;

[XmlInclude(typeof(WindowsTtsConfiguration))]
public abstract class TtsConfiguration : INotifyPropertyChanged
{
    public bool Enabled
    {
        get
        {
            return _enabled;
        }
        set
        {
            if (value == _enabled)
            {
                return;
            }

            _enabled = value;
            OnPropertyChanged();
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    private bool _enabled;

    [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
