using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using Brush = System.Drawing.Brush;

namespace Translumo.MVVM.Common
{
    public class BoolBrushConverter : IValueConverter
    {
        public SolidColorBrush TrueBrush { get; set; }

        public SolidColorBrush FalseBrush { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var boolValue = (bool)value;

            return boolValue ? TrueBrush : FalseBrush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
