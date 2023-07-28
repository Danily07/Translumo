using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Web.WebView2.Core;
using Newtonsoft.Json;
using Translumo.MVVM.Common;
using Translumo.Utils;

namespace Translumo.MVVM.Views
{
    /// <summary>
    /// Interaction logic for BrowserView.xaml
    /// </summary>
    public partial class BrowserView : Window
    {
        public static readonly DependencyProperty NotificationCaptionProperty =
            DependencyProperty.Register(
                "NotificationCaption", typeof(string), typeof(BrowserView),
                new FrameworkPropertyMetadata(
                    defaultValue: default(string), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, NotificationCaptionCallback));

        private static void NotificationCaptionCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ((string)e.NewValue == (string)e.OldValue)
            {
                return;
            }

            var targetControl = (BrowserView)d;
            targetControl.TbCaption.Text = (string)e.NewValue;
        }

        public string NotificationCaption
        {
            get { return (string)GetValue(NotificationCaptionProperty); }
            set { SetValue(NotificationCaptionProperty, value); }
        }

        public Guid SessionId { get; set; }

        public string TargetPageUrl { get; set; }

        public WebPageInfo TargetPageInfo { get; set; }

        public BrowserView(Guid sessionId)
        {
            InitializeComponent();

            this.SessionId = sessionId;
            this.Browser.NavigationCompleted += BrowserOnNavigationCompleted;
            this.Browser.CoreWebView2InitializationCompleted += BrowserOnCoreWebView2InitializationCompleted;
        }

        private void BrowserOnCoreWebView2InitializationCompleted(object sender, CoreWebView2InitializationCompletedEventArgs e)
        {
            Browser.CoreWebView2.CookieManager.DeleteAllCookies();
        }

        private async void BrowserOnNavigationCompleted(object sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            if (!string.IsNullOrEmpty(TargetPageUrl) && Browser.Source == new Uri(TargetPageUrl))
            {
                await this.ProcessClose();
            }
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            this.Topmost = true;
        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
            this.Topmost = true;
            this.Activate();
        }

        private async Task ProcessClose()
        {
            var htmlContent = await Browser.CoreWebView2.ExecuteScriptAsync("document.body.outerHTML");

            TargetPageInfo = new WebPageInfo()
            {
                Body = JsonConvert.DeserializeObject(htmlContent)?.ToString(),
                Cookies = (await Browser.CoreWebView2.CookieManager.GetCookiesAsync(TargetPageUrl)).Select(c => c.ToSystemNetCookie()).ToArray()
            };

            this.Close();
        }

        private void Window_SourceInitialized(object sender, EventArgs e)
        {
            WindowHelper.RemoveIcon(this);
            if (string.IsNullOrEmpty(NotificationCaption))
            {
                OverlayNotification.Visibility = Visibility.Hidden;
                //Overlay.Visibility = Visibility.Hidden;
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            OverlayNotification.Dispose();
            Browser.Dispose();
        }
    }
}
