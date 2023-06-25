using System;
using System.Threading;
using SharpDX.XInput;
using Translumo.HotKeys;
using Translumo.Utils;

namespace Translumo.Services
{
    public class GamepadService : IControllerService
    {
        public ObservablePipe<Keystroke> EventPipe { get; }

        public bool IsListening { get; private set; }

        private const int LISTENING_DELAY_MS = 60;

        private Thread _poolingThread;

        private readonly Controller _controller;

        public GamepadService(ObservablePipe<Keystroke> pipe)
        {
            _controller = new Controller(UserIndex.One);
            EventPipe = pipe;
        }

        public bool TryChangeListenState(bool enabled)
        {
            if (enabled == IsListening)
            {
                return true;
            }

            IsListening = enabled;
            if (enabled)
            {
                if (!_controller.IsConnected)
                {
                    IsListening = false;
                    return false;
                }

                _poolingThread = new Thread(ListenInternalLoop) { IsBackground = true };
                _poolingThread.Start();
            }
            else
            {
                _poolingThread.Join();
                _poolingThread = null;
            }

            return true;
        }

        private void ListenInternalLoop()
        {
            while (IsListening)
            {
                var result = _controller.GetKeystroke(DeviceQueryType.Gamepad, out var keystroke);
                if (result.Success)
                {
                    EventPipe.Send(keystroke);
                }

                Thread.Sleep(LISTENING_DELAY_MS);
            }
        }
    }
}
