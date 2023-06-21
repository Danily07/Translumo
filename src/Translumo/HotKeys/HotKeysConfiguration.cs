using System.Windows.Input;
using Translumo.Utils;

namespace Translumo.HotKeys
{
    public class HotKeysConfiguration : BindableBase
    {
        public static HotKeysConfiguration Default => new HotKeysConfiguration()
        {
            ChatVisibilityKey = new HotKeyInfo(Key.T, KeyModifier.Alt),
            SelectAreaKey = new HotKeyInfo(Key.Q, KeyModifier.Alt),
            SettingVisibilityKey = new HotKeyInfo(Key.G, KeyModifier.Alt),
            TranslationStateKey = new HotKeyInfo(Key.OemTilde, KeyModifier.None)
        };

        public HotKeyInfo ChatVisibilityKey
        {
            get => _chatVisibilityKey;
            set
            {
                SetProperty(ref _chatVisibilityKey, value);
            }
        }

        public HotKeyInfo TranslationStateKey
        {
            get => _translationStateKey;
            set
            {
                SetProperty(ref _translationStateKey, value);
            }
        }

        public HotKeyInfo SelectAreaKey
        {
            get => _selectAreaKey;
            set
            {
                SetProperty(ref _selectAreaKey, value);
            }
        }

        public HotKeyInfo SettingVisibilityKey
        {
            get => _settingVisibilityKey;
            set
            {
                SetProperty(ref _settingVisibilityKey, value);
            }
        }

        private HotKeyInfo _chatVisibilityKey;
        private HotKeyInfo _selectAreaKey;
        private HotKeyInfo _translationStateKey;
        private HotKeyInfo _settingVisibilityKey;
    }
}
