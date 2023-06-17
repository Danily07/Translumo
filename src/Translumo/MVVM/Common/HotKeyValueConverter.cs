using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Input;
using Translumo.HotKeys;

namespace Translumo.MVVM.Common
{
    public class HotKeyValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is HotKeyInfo))
            {
                return string.Empty;
            }

            var keyInfo = (HotKeyInfo)value;

            return (keyInfo.KeyModifier != KeyModifier.None ? $"{keyInfo.KeyModifier}+" : string.Empty) +
                   KeyToString(keyInfo.Key);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private string KeyToString(Key key)
        {
            if (key == Key.OemTilde)
            {
                return "~";
            }

            return key.ToString();
        }
    }
}
