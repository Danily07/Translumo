using System.Windows.Input;
using SharpDX.XInput;
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
            TranslationStateKey = new HotKeyInfo(Key.OemTilde, KeyModifier.None),

            ChatVisibilityGamepadKey = new GamepadHotKeyInfo(GamepadKeyCode.None),
            SelectAreaGamepadKey = new GamepadHotKeyInfo(GamepadKeyCode.None),
            SettingVisibilityGamepadKey = new GamepadHotKeyInfo(GamepadKeyCode.None),
            TranslationStateGamepadKey = new GamepadHotKeyInfo(GamepadKeyCode.None)
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

        public GamepadHotKeyInfo ChatVisibilityGamepadKey
        {
            get => _chatVisibilityGamepadKey;
            set
            {
                SetProperty(ref _chatVisibilityGamepadKey, value);
            }
        }

        public GamepadHotKeyInfo TranslationStateGamepadKey
        {
            get => _translationStateGamepadKey;
            set
            {
                SetProperty(ref _translationStateGamepadKey, value);
            }
        }

        public GamepadHotKeyInfo SelectAreaGamepadKey
        {
            get => _selectAreaGamepadKey;
            set
            {
                SetProperty(ref _selectAreaGamepadKey, value);
            }
        }

        public GamepadHotKeyInfo SettingVisibilityGamepadKey
        {
            get => _settingVisibilityGamepadKey;
            set
            {
                SetProperty(ref _settingVisibilityGamepadKey, value);
            }
        }

        private HotKeyInfo _chatVisibilityKey;
        private HotKeyInfo _selectAreaKey;
        private HotKeyInfo _translationStateKey;
        private HotKeyInfo _settingVisibilityKey;

        private GamepadHotKeyInfo _chatVisibilityGamepadKey;
        private GamepadHotKeyInfo _selectAreaGamepadKey;
        private GamepadHotKeyInfo _translationStateGamepadKey;
        private GamepadHotKeyInfo _settingVisibilityGamepadKey;
    }
}
