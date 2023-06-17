using Translumo.HotKeys;
using Translumo.Utils;

namespace Translumo.MVVM.Models
{
    public class DisplayHotKey : BindableBase
    {
        public HotKeyInfo HotKey
        {
            get => _hotKey;
            set => SetProperty(ref _hotKey, value);
        }

        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        private HotKeyInfo _hotKey;
        private string _description;

        public DisplayHotKey(HotKeyInfo hotKey, string description)
        {
            _hotKey = hotKey;
            _description = description;
        }
    }
}
