using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Translumo.Translation.Papago
{
    internal class PapagoResponse
    {
        [JsonPropertyName("srcLangType")]
        public string SourceLanguageType { get; set; }
        [JsonPropertyName("tarLangType")]
        public string TargetLanguageType { get; set; }
        [JsonPropertyName("translatedText")]
        public string TranslatedText { get; set; }
        [JsonPropertyName("delay")]
        public int Delay { get; set; }
        [JsonPropertyName("delaySmt")]
        public int DelaySmt { get; set; }
        [JsonPropertyName("engineType")]
        public string EngineType { get; set; }
        [JsonPropertyName("langDetection")]
        public LangDetection LanguageDetection { get; set; }

        internal sealed class Nbest
        {
            [JsonPropertyName("lang")]
            public string Lang { get; set; }
            [JsonPropertyName("prob")]
            public double Prob { get; set; }
        }

        internal sealed class LangDetection
        {
            [JsonPropertyName("nbests")]
            public List<Nbest> Nbests { get; set; }
        }
    }
}
