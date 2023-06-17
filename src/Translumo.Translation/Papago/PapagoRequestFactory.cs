namespace Translumo.Translation.Papago
{
    internal static class PapagoRequestFactory
    {
        public static PapagoRequest CreateRequest(PapagoContainer container, string text, string sourceLangCode, string targetLangCode)
        {
            container.Guid = string.IsNullOrEmpty(container.Guid) ? PapagoEncodingProvider.GetGuid() : container.Guid;
            string timestamp = PapagoEncodingProvider.GetTimestamp().ToString();
            string authPostfix = PapagoEncodingProvider.GetHmacPostfix(container.Guid, timestamp, container.AuthKey);

            return new PapagoRequest()
            {
                AuthorizationHeader = $"PPG {container.Guid}:{authPostfix}",
                TimestampHeader = timestamp,
                Body = new PapagoRequest.PapagoRequestBody()
                {
                    DeviceId = container.Guid,
                    Locale = "en",
                    Dict = false,
                    DictDisplay = 0L,
                    Honorific = false,
                    Instant = false,
                    Paging = false,
                    Source = sourceLangCode,
                    Target = targetLangCode,
                    Text = text
                }
            };
        }
    }
}
