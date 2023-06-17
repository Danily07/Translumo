using System;
using System.Globalization;
using System.Windows.Data;

namespace Translumo.MVVM.Common
{
    internal class SliderPercentValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((1.0 - (float)value) * 10);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var val = (double)value;
            return (float)(1 - val / 10);
        }
    }
}
