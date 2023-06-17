using System;
using System.Drawing;
using Point = System.Windows.Point;

namespace Translumo.Configuration
{
    public class ScreenCaptureConfiguration
    {
        public Point CaptureAreaP1
        {
            get => _captureAreaP1;
            set
            {
                _captureAreaP1 = value;
                RecalculateArea();
            }
        }

        public Point CaptureAreaP2
        {
            get => _captureAreaP2;
            set
            {
                _captureAreaP2 = value;
                RecalculateArea();
            }
        }

        public RectangleF CaptureArea { get; private set; }

        private Point _captureAreaP1;
        private Point _captureAreaP2;

        private void RecalculateArea()
        {
            CaptureArea = new RectangleF((int)Math.Min(CaptureAreaP1.X, CaptureAreaP2.X),
                (int)Math.Min(CaptureAreaP1.Y, CaptureAreaP2.Y),
                (int)Math.Abs(CaptureAreaP1.X - CaptureAreaP2.X),
                (int)Math.Abs(CaptureAreaP1.Y - CaptureAreaP2.Y));
        }
    }
}
