namespace Translumo.Translation.Papago
{
    public class PapagoRequest
    {
        public string TimestampHeader { get; set; }

        public string AuthorizationHeader { get; set; }

        public PapagoRequestBody Body { get; set; }

        public sealed class PapagoRequestBody
        {
            public string DeviceId { get; set; }

            public string Locale { get; set; }

            public bool Dict { get; set; }

            public long DictDisplay { get; set; }

            public bool Honorific { get; set; }

            public bool Instant { get; set; }

            public bool Paging { get; set; }

            public string Source { get; set; }

            public string Target { get; set; }

            public string Text { get; set; }
        }
    }
}
