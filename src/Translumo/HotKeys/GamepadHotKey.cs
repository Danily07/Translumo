using System;
using SharpDX.XInput;

namespace Translumo.HotKeys
{
    public class GamepadHotKey
    {

        public int Id => (int)KeyCode;

        public GamepadKeyCode KeyCode { get; set; }

        public Action Action { get; set; }

        public GamepadHotKey(GamepadKeyCode keyCode, Action action)
        {
            Action = action;
            KeyCode = keyCode;
        }

        public override bool Equals(object obj)
        {
            var anotherHotKey = obj as GamepadHotKey;
            if (anotherHotKey == null)
            {
                return false;
            }

            return Id.Equals(anotherHotKey.Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
