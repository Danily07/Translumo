using System.Drawing;
using System.Drawing.Imaging;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using Translumo.Infrastructure.Language;
using Size = OpenCvSharp.Size;

namespace Translumo.OCR.WindowsOCR
{
    public class WinOCREngineWithPreprocess : WindowsOCREngine
    {
        public override int Confidence => 4;
        public override byte PrimaryPriority => 5;

        public WinOCREngineWithPreprocess(LanguageDescriptor languageDescriptor) : base(languageDescriptor)
        {
            base.ImgFormat = ImageFormat.Bmp;
        }

        protected override Bitmap PreProcess(Bitmap bitmap)
        {
            using (var src = bitmap.ToMat())
            {
                var kernel = Cv2.GetStructuringElement(MorphShapes.Rect, new Size(1, 1));
                Cv2.CvtColor(src, src, ColorConversionCodes.BGR2GRAY);
                Cv2.Threshold(src, src, 150, 255, ThresholdTypes.Binary);
                Cv2.MorphologyEx(src, src, MorphTypes.Open, kernel);

                return src.ToBitmap();
            }
        }
    }
}
