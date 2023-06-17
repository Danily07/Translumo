using System;
using System.Globalization;
using System.Windows.Data;

namespace Translumo.MVVM.Common
{
    public class SumValueConverter : IValueConverter
    {
        public int Arg { get; set; }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((int)value) + Arg;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
