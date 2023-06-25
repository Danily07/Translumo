using System;

namespace Translumo.Processing.Interfaces
{
    public interface IScreenCapturer : IDisposable
    {
        int CaptureAttempts { get; set; }

        void Initialize();
        byte[] CaptureScreen();
    }
}
