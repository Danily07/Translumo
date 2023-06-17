using System;
using System.Windows.Controls;
using System.Windows.Interop;
using Translumo.Utils;

namespace Translumo.Controls
{
    internal class OverlayControl : ContentControl, IDisposable
    {
        private OverlayWindow _window;
        private HwndHostEx _host;

        public OverlayControl()
        {
            Loaded += OverlayControl_Loaded;
        }

        private void OverlayControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            var content = Content;

            _window = new OverlayWindow
            {
                Content = content
            };
            _window.Show();

            IntPtr windowHandle = new WindowInteropHelper(_window).Handle;

            _host = new HwndHostEx(windowHandle);
            Content = _host;
        }

        public void Dispose()
        {
            _host?.Dispose();
            _window?.Close();
        }
    }
}
