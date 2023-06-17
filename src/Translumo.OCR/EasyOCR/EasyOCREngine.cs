using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Python.Runtime;
using Translumo.Infrastructure.Constants;
using Translumo.Infrastructure.Language;

namespace Translumo.OCR.EasyOCR
{
    public class EasyOCREngine : IOCREngine, IDisposable
    {
        public byte PrimaryPriority => 2;
        public bool SecondaryPrimaryCheck => false;
        public int Confidence => 9;
        public Languages DetectionLanguage => _languageDescriptor.Language;

        private IntPtr _threadState;
        private bool _objectsInitialized;
        private bool _readerIsUsed;

        private readonly object _obj = new object(); 
        private readonly LanguageDescriptor _languageDescriptor;
        private readonly string _modelPath = Path.Combine(Global.ModelsPath, "easyocr");
        private readonly ILogger _logger;

        #region Python objects
        private PyObject _builtinsLib;
        private dynamic _easyOcrLib;
        private dynamic _reader;
        private dynamic _bytes;
        #endregion

        public EasyOCREngine(LanguageDescriptor languageDescriptor, ILogger logger)
        {
            Runtime.PythonDLL = Path.Combine(Global.PythonPath, "python38.dll");
            PythonEngine.PythonHome = Global.PythonPath;
            _languageDescriptor = languageDescriptor;
            _logger = logger;

            _logger.LogTrace($"Initialization EasyOCR from path: '{Global.PythonPath}'");

            Task.Factory.StartNew(EnsureInitialized);
        }

        public string[] GetTextLines(byte[] image)
        {
            Thread.CurrentThread.Priority = ThreadPriority.AboveNormal;
            lock (_obj)
            {
                if (!_objectsInitialized)
                {
                    throw new InvalidOperationException($"EasyOCR is not initialized");
                }

                using (Py.GIL())
                {
                    dynamic ocrResult = _reader.readtext(_bytes.Invoke(image.ToPython()), detail: 0, paragraph: true);
                    _readerIsUsed = true;

                    return (string[])ocrResult;
                }
            }
        }

        private void EnsureInitialized()
        {
            lock (_obj)
            {
                try
                {
                    if (!PythonEngine.IsInitialized)
                    {
                        PythonEngine.Initialize();
                        _threadState = PythonEngine.BeginAllowThreads();
                    }

                    if (!_objectsInitialized)
                    {
                        InitializeObjects();
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"EasyOCR initialization error");
                }
            }
        }

        private void InitializeObjects()
        {
            using (Py.GIL())
            {
                _builtinsLib = Py.Import("builtins");
                _easyOcrLib = Py.Import("easyocr");
                _reader = _easyOcrLib.Reader(new[] { _languageDescriptor.EasyOcrCode }, model_storage_directory: _modelPath, download_enabled: false, recog_network: _languageDescriptor.EasyOcrModel);
                _bytes = _builtinsLib.GetAttr("bytes");

                _objectsInitialized = true;
            }
        }

        public void Dispose()
        {
            lock (_obj)
            {
                if (_objectsInitialized)
                {
                    _builtinsLib.Dispose();
                    _easyOcrLib.Dispose();
                    if (_readerIsUsed)
                    {
                        _reader.Dispose();
                    }

                    _readerIsUsed = false;
                    _objectsInitialized = false;
                }

                if (PythonEngine.IsInitialized)
                {
                    //Causes PythonEngine.Shutdown() hanging (https://github.com/pythonnet/pythonnet/issues/1701)
                    //PythonEngine.EndAllowThreads(_threadState);
                    PythonEngine.Shutdown();
                }
            }
        }
    }
}
