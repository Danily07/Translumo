using System;
using System.Linq;
using Translumo.Utils.Http;

namespace Translumo.Translation.Yandex
{
    public class YandexRequestFactory
    {
        public static YandexRequest CreateRequest(YandexContainer container, string text, string sourceLangCode, string targetLangCode)
        {
            string yandexUid = GetYandexUid(container.Reader.HttpReader, container.Reader.YandexRuUrl);
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
                    Yu = yandexUid
                },
                Body = new YandexRequest.YandexRequestBody()
                {
                    Options = 4,
                    Text = text
                }
            };
        }

        private static string GetYandexUid(HttpReader yandexReader, string yandexUrl)
        {
            return yandexReader.Cookies
                .GetCookies(new Uri(yandexUrl))
                .FirstOrDefault(cookie => cookie.Name == "yandexuid")
                ?.Value;
        }
    }
}
