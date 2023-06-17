using System;

namespace Translumo.Processing.Exceptions
{
    public class CaptureException : Exception
    {
        public int? ErrorCode { get; }

        public CaptureException(string message) : base(message)
        {
        }

        public CaptureException(string message, int errorCode) : base(message)
        {
            this.ErrorCode = errorCode;
        }

        public CaptureException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
