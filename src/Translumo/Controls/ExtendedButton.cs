using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using MaterialDesignThemes.Wpf;

namespace Translumo.Controls
{
    public class ExtendedButton : Button
    {
        public static readonly DependencyProperty IconKindProperty = DependencyProperty.Register(nameof(IconKind), typeof(PackIconKind), typeof(ExtendedButton));
        public static readonly DependencyProperty CaptionProperty = DependencyProperty.Register(nameof(Caption), typeof(string), typeof(ExtendedButton));



        [Description("The icon kind of button"), Category("Common")]
        public PackIconKind IconKind
        {
            get => (PackIconKind)GetValue(IconKindProperty);
            set => SetCurrentValue(IconKindProperty, value);
        }

        [Description("The button caption"), Category("Common")]
        public string Caption
        {
            get => (string)GetValue(CaptionProperty);
            set => SetCurrentValue(CaptionProperty, value);
        }

        public ExtendedButton()
        {
        }

        static ExtendedButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ExtendedButton), new FrameworkPropertyMetadata(typeof(ExtendedButton)));
        }
    }
}
