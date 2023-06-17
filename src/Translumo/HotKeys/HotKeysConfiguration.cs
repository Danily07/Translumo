using System.Windows.Input;

namespace Translumo.HotKeys
{
    public class HotKeysConfiguration
    {
        public static HotKeysConfiguration Default => new HotKeysConfiguration()
        {
            ChatVisibilityKey = new HotKeyInfo(Key.T, KeyModifier.Alt),
            SelectAreaKey = new HotKeyInfo(Key.Q, KeyModifier.Alt),
            SettingVisibilityKey = new HotKeyInfo(Key.G, KeyModifier.Alt),
            TranslationStateKey = new HotKeyInfo(Key.OemTilde, KeyModifier.None)
        };

        public HotKeyInfo ChatVisibilityKey { get; set; }

        public HotKeyInfo TranslationStateKey { get; set; }

        public HotKeyInfo SelectAreaKey { get; set; }

        public HotKeyInfo SettingVisibilityKey { get; set; }
    }
}
