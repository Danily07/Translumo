using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Translumo.MVVM.Common
{
    public class VisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is bool))
                return null;
            return (bool)value ? Visibility.Visible : Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (Equals(value, Visibility.Visible))
            {
                return true;
            }

            if (Equals(value, Visibility.Hidden))
            {
                return false;
            }

            return null;
        }
    }
}
