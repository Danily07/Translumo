namespace Translumo.Translation.Yandex
{
    public class YandexRequest
    {
        public YandexRequestQuery QueryParams { get; set; }

        public YandexRequestBody Body { get; set; }

        public class YandexRequestQuery
        {
            public string Id { get; set; }

            public string Srv { get; set; }

            public string Source_lang { get; set; }

            public string Target_lang { get; set; }

            public string Reason { get; set; }

            public string Format { get; set; }

            public int Ajax { get; set; }

            public string Yu { get; set; }
        }

        public class YandexRequestBody
        {
            public string Text { get; set; }

            public int Options { get; set; }
        }
    }
}
