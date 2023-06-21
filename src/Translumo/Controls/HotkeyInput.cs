using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Translumo.MVVM.Common;
using Translumo.Utils;

namespace Translumo.Controls
{
    internal class HotkeyInput : TextBox
    {
        public static readonly DependencyProperty HotKeyProperty = DependencyProperty.Register(nameof(HotKey), typeof(KeyCombination), typeof(HotkeyInput), new PropertyMetadata(OnHotKeyChanged));

        public KeyCombination HotKey
        {
            get => (KeyCombination)GetValue(HotKeyProperty);
            set => SetCurrentValue(HotKeyProperty, value);
        }

        private Key? _pressedKey;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            PreviewKeyUp += OnPreviewKeyUp;
            PreviewKeyDown += OnPreviewKeyDown;
            GotFocus += OnGotFocus;
            LostFocus += OnLostFocus;
        }


        private void OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.IsRepeat)
            {
                return;
            }

            var key = _pressedKey ?? e.GetActualKey();
            HotKey = new KeyCombination { Key = key, Modifier = e.KeyboardDevice.Modifiers };
            e.Handled = true;
            _pressedKey = (int)e.Key > 115 ? null : e.GetActualKey();
        }

        private void OnLostFocus(object sender, RoutedEventArgs e)
        {
            _pressedKey = null;
            this.Text = HotKey.ToString();
        }

        private void OnGotFocus(object sender, RoutedEventArgs e)
        {
            this.Text = "...";
        }

        private void OnPreviewKeyUp(object sender, KeyEventArgs e)
        {
            LoseFocus();
        }

        private static void OnHotKeyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            if (sender is HotkeyInput input)
            {
                input.Text = ((KeyCombination)args.NewValue).ToString();
            }
        }

        private void LoseFocus()
        {
            FocusManager.SetFocusedElement(FocusManager.GetFocusScope(this), null);
            Keyboard.ClearFocus();
        }
    }
}
