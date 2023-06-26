using System.Windows.Input;

namespace Translumo.Utils
{
    public static class KeyEventArgsExtensions
    {
        public static Key GetActualKey(this KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.System:
                    return e.SystemKey;

                case Key.ImeProcessed:
                    return e.ImeProcessedKey;

                case Key.DeadCharProcessed:
                    return e.DeadCharProcessedKey;

                default:
                    return e.Key;
            }
        }
    }
}
