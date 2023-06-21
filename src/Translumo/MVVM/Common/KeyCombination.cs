using System.Windows.Input;

namespace Translumo.MVVM.Common
{
    public struct KeyCombination
    {
        public Key Key;
        public ModifierKeys Modifier;

        public override string ToString()
        {
            if (Modifier == ModifierKeys.None)
            {
                return Key.ToString();
            }


            return $"{Modifier}+{Key}";
        }
    }
}
