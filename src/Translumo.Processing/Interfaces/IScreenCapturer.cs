using System;

namespace Translumo.Processing.Interfaces
{
    public interface IScreenCapturer : IDisposable
    {
        int CaptureAttempts { get; set; }
        bool HasCaptureArea { get; }

        void Initialize();
        byte[] CaptureScreen();
    }
}
