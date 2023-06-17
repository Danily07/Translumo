using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Translumo.Infrastructure.Constants;
using Translumo.Infrastructure.Language;
using Translumo.OCR;
using Translumo.Processing.Exceptions;
using static System.Threading.Tasks.Task;

namespace Translumo.Processing.TextProcessing
{
    public class TextDetectionProvider : IDisposable
    {
        public Languages Language
        {
            get => _textValidityPredictor.Language;
            set => _textValidityPredictor.Language = value;
        }

        private readonly TextValidityPredictor _textValidityPredictor;

        
        public TextDetectionProvider(TextValidityPredictor textValidityPredictor)
        {
            this._textValidityPredictor = textValidityPredictor;
        }

        public virtual TextDetectionResult GetText(IOCREngine ocrEngine, byte[] img)
        {
            try
            {
                var detectedLines = ocrEngine.GetTextLines(img);
                var resultText = PreProcessTextLines(detectedLines);

                var scorePrediction = _textValidityPredictor.Predict(detectedLines, out var validatedText);

                return new TextDetectionResult(ocrEngine)
                {
                    ValidityScore = scorePrediction,
                    Text = resultText,
                    ValidatedText = validatedText
                };
            }
            catch (Exception ex)
            {
                throw new TextDetectionException("Text detection failed", ocrEngine.GetType(), ex);
            }
        }

        public virtual Task<TextDetectionResult> GetTextAsync(IOCREngine ocrEngine, byte[] img)
        {
            return Factory.StartNew(() => GetText(ocrEngine, img));
        }

        private string PreProcessTextLines(IEnumerable<string> textLines)
        {
            return RegexStorage.MultipleSpacesRegex.Replace(string.Join(' ', textLines), " ");
        }

        public void Dispose()
        {
            _textValidityPredictor.Dispose();
        }
    }
}
