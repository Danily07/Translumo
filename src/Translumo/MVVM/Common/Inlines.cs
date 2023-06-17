using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Xml;

namespace Translumo.MVVM.Common
{
    public class Inlines
    {
        public static readonly DependencyProperty TextProperty = DependencyProperty.RegisterAttached("Text",
            typeof(string), typeof(Inlines), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.AffectsMeasure, OnTextPropertyChanged));

        public static void SetText(DependencyObject @do, string value)
        {
            @do.SetValue(TextProperty, value);
        }

        public static string GetText(DependencyObject @do)
        {
            return (string)@do.GetValue(TextProperty);
        }

        private static void OnTextPropertyChanged(DependencyObject @do, DependencyPropertyChangedEventArgs e)
        {
            var textBlock = @do as TextBlock;

            if (textBlock == null)
            {
                throw new InvalidOperationException("This property may only be set on TextBox");
            }

            var value = GetText(@do);

            var text = "<Span xml:space=\"preserve\" xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\">" +
                       $"{value ?? string.Empty}</Span>";

            textBlock.Inlines.Clear();

            using (var xmlReader = XmlReader.Create(new StringReader(text)))
            {
                var result = (Span)XamlReader.Load(xmlReader);
                textBlock.Inlines.Add(result);
            }
        }
    }
}
