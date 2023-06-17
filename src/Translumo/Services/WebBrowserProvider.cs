using System;
using System.Collections.Concurrent;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Web.WebView2.Core;
using Translumo.MVVM.Common;
using Translumo.MVVM.Views;
using Translumo.Utils.Extensions;

namespace Translumo.Services
{
    public static class WebBrowserProvider
    {
        private static ConcurrentDictionary<Guid, ManualResetEvent> _browserSessions = new();

        public static async Task<WebPageInfo> BrowsePageAsync(string sourceUrl, string targetUrl, CancellationToken cancellationToken,
            WebProxy proxy = null, string notificationDescription = null)
        {
            var sessionId = Guid.NewGuid();
            try
            {
                _browserSessions[sessionId] = new ManualResetEvent(false);
                var browserView = await CreateBrowserView(sessionId, sourceUrl, targetUrl, proxy, notificationDescription);
                browserView.Show();

                await _browserSessions[sessionId].WaitOneAsync(cancellationToken);

                return browserView.TargetPageInfo;
            }
            finally
            {
                _browserSessions.TryRemove(sessionId, out var value);
            }
        }

        private static async Task<BrowserView> CreateBrowserView(Guid sessionId, string sourceAddress, string targetUrl, WebProxy proxy = null, string notificationDescription = null)
        {
            var browserView = new BrowserView(sessionId);
            browserView.TargetPageUrl = targetUrl;
            browserView.Browser.Source = new Uri(sourceAddress);
            browserView.NotificationCaption = notificationDescription;
            if (proxy != null)
            {
                var credential = proxy.Credentials as NetworkCredential;
                browserView.Browser.CoreWebView2.BasicAuthenticationRequested += (sender, args) =>
                {
                    args.Response.UserName = credential.UserName;
                    args.Response.Password = credential.Password;
                };
                var options = new CoreWebView2EnvironmentOptions(additionalBrowserArguments: $"--proxy-server=\"{proxy.Address.Host}:{proxy.Address.Port}\"");
                var env = await CoreWebView2Environment.CreateAsync(options: options);
                await browserView.Browser.EnsureCoreWebView2Async(env);
            }
            
            browserView.Closed += BrowserViewOnClosed;

            return browserView;
        }

        private static void BrowserViewOnClosed(object sender, EventArgs e)
        {
            var view = sender as BrowserView;
            _browserSessions[view.SessionId].Set();
        }
    }
}
