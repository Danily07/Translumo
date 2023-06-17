using System;
using System.Drawing;
using System.IO;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using Tesseract;
using Translumo.Infrastructure.Constants;
using Translumo.Infrastructure.Language;

namespace Translumo.OCR.Tesseract
{
    public class 
        TesseractOCREngine : IOCREngine, IDisposable
    {
        public bool SecondaryPrimaryCheck => true;
        public Languages DetectionLanguage => _languageDescriptor.Language;

        public virtual int Confidence => 1;
        public virtual byte PrimaryPriority => 3;

        protected TesseractEngine Engine;
        protected bool IsInitialized;

        protected const string LINES_SEPARATOR = "\n";

        private readonly string _dataPath = Path.Combine(Global.ModelsPath, "tessdata");
        private readonly LanguageDescriptor _languageDescriptor;

        public TesseractOCREngine(LanguageDescriptor languageDescriptor)
        {
            _languageDescriptor = languageDescriptor;
        }

        public string[] GetTextLines(byte[] image)
        {
            if (!IsInitialized)
            {
                Initialize();
            }

            using (var img = Pix.LoadTiffFromMemory(PreProcess(image)))
            {
                using (var page = Engine.Process(img))
                {
                    return page.GetText().Split(LINES_SEPARATOR);
                }
            }
        }

        public void Dispose()
        {
            Engine?.Dispose();
        }

        protected virtual Byte[] PreProcess(byte[] image)
        {
            using var stream = new MemoryStream(image);
            var bitmap = new Bitmap(stream);
            using (var src = bitmap.ToMat())
            {
                if (src.Channels() != 1)
                {
                    Cv2.CvtColor(src, src, ColorConversionCodes.BGR2GRAY);
                }
                return src.ToBytes(".tif");
            }
        }

        protected void Initialize()
        {
            Engine = new TesseractEngine(_dataPath, _languageDescriptor.TesseractCode, EngineMode.LstmOnly);
            Engine.DefaultPageSegMode = PageSegMode.SingleBlock;

            if (_languageDescriptor.Language == Languages.Chinese || _languageDescriptor.Language == Languages.Japanese)
            {
                Engine.SetVariable("preserve_interword_spaces", 1);
                Engine.SetVariable("chop_enable", true);
                Engine.SetVariable("use_new_state_cost", false);
                Engine.SetVariable("segment_segcost_rating", false);
                Engine.SetVariable("enable_new_segsearch", 0);
                Engine.SetVariable("language_model_ngram_on", 0);
                Engine.SetVariable("textord_force_make_prop_words", false);
                Engine.SetVariable("edges_max_children_per_outline", 40);
            }

            IsInitialized = true;
        }
    }
}
