using System;

namespace Translumo.Processing.Exceptions
{
    public class TextDetectionException : Exception
    {
        public Type SourceOCREngineType { get; }

        public TextDetectionException(string message, Type type) : base(message)
        {
            this.SourceOCREngineType = type;
        }

        public TextDetectionException(string message, Type type, Exception innerException) : base(message, innerException)
        {
            this.SourceOCREngineType = type;
        }
    }
}
