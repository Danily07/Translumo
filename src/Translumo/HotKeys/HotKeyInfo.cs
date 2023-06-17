using System.Windows.Input;

namespace Translumo.HotKeys
{
    public class HotKeyInfo
    {
        public Key Key { get; set; }

        public KeyModifier KeyModifier { get; set; }

        public HotKeyInfo(Key key, KeyModifier keyModifier)
        {
            Key = key;
            KeyModifier = keyModifier;
        }
    }
}
