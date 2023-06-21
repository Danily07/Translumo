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

        public override bool Equals(object obj)
        {
            var anotherHotKey = obj as HotKeyInfo;
            if (anotherHotKey == null)
            {
                return false;
            }

            return this.Key == anotherHotKey.Key && this.KeyModifier == anotherHotKey.KeyModifier;
        }
    }
}
