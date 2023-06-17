using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Translumo.MVVM.Common
{
    internal class BoldFontConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var isBold = (bool)value;

            return isBold ? FontWeights.Bold : FontWeights.Normal;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var fontWeight = (FontWeight)value;

            return fontWeight == FontWeights.Bold;
        }
    }
}
