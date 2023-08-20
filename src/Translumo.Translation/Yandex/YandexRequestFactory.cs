using System;
using System.Linq;
using Translumo.Utils.Http;

namespace Translumo.Translation.Yandex
{
    public class YandexRequestFactory
    {
        public static YandexRequest CreateRequest(YandexContainer container, string text, string sourceLangCode, string targetLangCode)
        {
            string yandexUid = GetYandexCookieValue(container.Reader, "yandexuid");
            string yandexYum = GetYandexCookieValue(container.Reader, "_ym_uid");
            string yandexSpravka = GetYandexCookieValue(container.Reader, "spravka");

            return new YandexRequest()
            {
                QueryParams = new YandexRequest.YandexRequestQuery()
                {
                    Id = $"{container.Sid}-{container.GetRequestNumber()}-0",
                    Srv = "tr-text",
                    Source_lang = sourceLangCode,
                    Target_lang = targetLangCode,
                    Reason = "auto",
                    Format = "text",
                    Ajax = 1,
                    Yu = yandexUid,
                    Yum = yandexYum,
                    Sprvk = yandexSpravka
                },
                Body = new YandexRequest.YandexRequestBody()
                {
                    Options = 4,
                    Text = text
                }
            };
        }

        private static string GetYandexCookieValue(YandexReaderProxy yandexReader, string cookieName)
        {
            return yandexReader.HttpReader.Cookies
                .GetCookies(yandexReader.YandexRuUri)
                .FirstOrDefault(cookie => cookie.Name == cookieName)
                ?.Value;
        }
    }
}
