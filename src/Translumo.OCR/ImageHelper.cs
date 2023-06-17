using System.Drawing;
using System.IO;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using Size = OpenCvSharp.Size;

namespace Translumo.OCR
{
    public static class ImageHelper
    {
        public static Mat ToGrayScale(byte[] image)
        {
            using var stream = new MemoryStream(image);
            var bitmap = new Bitmap(stream);
            var src = bitmap.ToMat();
            //using (var src = bitmap.ToMat())
            {
                var kernel = Cv2.GetStructuringElement(MorphShapes.Rect, new Size(1, 1));
                Cv2.CvtColor(src, src, ColorConversionCodes.BGR2GRAY);
                Cv2.Threshold(src, src, 150, 255, ThresholdTypes.Binary);
                Cv2.MorphologyEx(src, src, MorphTypes.Open, kernel);

                return src;
            }
        }

        public static byte[] UnionImages(Mat image, Mat image2)
        {
            using (var res = new Mat(image.Size(), image.Type()))
            {
                Cv2.BitwiseAnd(image, image2, res);

                return res.ToBytes(".tif");
            }
        }
    }
}
