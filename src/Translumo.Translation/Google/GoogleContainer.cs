using System.Net;
using Translumo.Translation.Configuration;
using Translumo.Utils.Http;

namespace Translumo.Translation.Google
{
    public sealed class GoogleContainer : TranslationContainer
    {
        public HttpReader Reader { get; set; }

        public GoogleContainer(Proxy proxy = null, bool isPrimary = false) : base(proxy, isPrimary)
        {
            Reader = CreateReader(proxy);
        }

        public override void Block()
        {
            base.Block();
            Reader.Cookies = new CookieContainer();
        }

        private HttpReader CreateReader(Proxy proxy)
        {
            var httpReader = new HttpReader();
            httpReader.Proxy = proxy?.ToWebProxy();

            httpReader.ContentType = null;
            httpReader.UserAgent = "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/41.0.2228.0 Safari/537.36";
            httpReader.Accept = "*/*";

            httpReader.OptionalHeaders.Add("Accept-Language", "en-US;q=0.8,en;q=0.7");
            httpReader.OptionalHeaders.Add("Upgrade-Insecure-Requests", "1");
            httpReader.OptionalHeaders.Add("Cache-Control", "no-cache");
            httpReader.OptionalHeaders.Add("DNT", "1");

            return httpReader;
        }
    }
}
