using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Translumo.Translation.Deepl
{
    public class DeepLResponse
    {
        internal sealed class DeepLTranslationResponse
        {
            [JsonPropertyName("result")]
            public Result Result { get; set; }
        }

        public sealed class DeepLHandshakeResponse
        {
            [JsonPropertyName("jsonrpc")]
            public string Jsonrpc { get; set; }
            [JsonPropertyName("result")]
            public ResultEntity Result { get; set; }
            [JsonPropertyName("id")]
            public int Id { get; set; }

            public sealed class ResultEntity
            {
                [JsonPropertyName("ip")]
                public string Ip { get; set; }
                [JsonPropertyName("proAvailable")]
                public bool ProAvailable { get; set; }
                [JsonPropertyName("updateNecessary")]
                public bool UpdateNecessary { get; set; }
                [JsonPropertyName("ep")]
                public bool Ep { get; set; }
            }
        }

        public sealed class DeepLSentencePreprocessResponse
        {
            [JsonPropertyName("id")]
            public int Id { get; set; }
            [JsonPropertyName("jsonrpc")]
            public string Jsonrpc { get; set; }
            [JsonPropertyName("result")]
            public ResultEntity Result { get; set; }

            public sealed class ResultEntity
            {
                [JsonPropertyName("lang")]
                public string Language { get; set; }
                [JsonPropertyName("lang_is_confident")]
                public int LanguageIsConfident { get; set; }
                [JsonPropertyName("splitted_texts")]
                public List<List<string>> SplittedTexts { get; set; }
            }
        }

        public sealed class Translation
        {
            [JsonPropertyName("beams")]
            public List<Beam> Beams { get; set; }
        }

        public sealed class Result
        {
            [JsonPropertyName("translations")]
            public List<Translation> Translations { get; set; }
        }

        public sealed class Beam
        {
            [JsonPropertyName("postprocessed_sentence")]
            public string PostProcessedSentence { get; set; }
        }
    }
}
