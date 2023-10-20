using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Translumo.Infrastructure.Language;
using Translumo.Infrastructure.Python;
using Translumo.OCR.Configuration;
using Translumo.OCR.EasyOCR;
using Translumo.OCR.Tesseract;
using Translumo.OCR.WindowsOCR;

namespace Translumo.OCR
{
    public class OcrEnginesFactory
    {
        private IList<IOCREngine> _cachedEngines;

        private readonly LanguageService _languageService;
        private readonly PythonEngineWrapper _pythonEngine;
        private readonly ILogger _logger;

        public OcrEnginesFactory(LanguageService languageService, PythonEngineWrapper pythonEngine, ILogger<OcrEnginesFactory> logger)
        {
            this._languageService = languageService;
            _pythonEngine = pythonEngine;
            this._logger = logger;
            this._cachedEngines = new List<IOCREngine>();
        }
        public IEnumerable<IOCREngine> GetEngines(IEnumerable<OcrConfiguration> ocrConfigurations,
            Languages detectionLanguage)
        {
            var langDescriptor = _languageService.GetLanguageDescriptor(detectionLanguage);

            foreach (var ocrConfiguration in ocrConfigurations)
            {
                var confType = ocrConfiguration.GetType();

                if (confType == typeof(WindowsOCRConfiguration))
                {
                    if (!TryRemoveIfDisabled<WindowsOCREngine>(ocrConfiguration))
                        yield return GetEngine(() => new WindowsOCREngine(langDescriptor), detectionLanguage);

                    if (!TryRemoveIfDisabled<WinOCREngineWithPreprocess>(ocrConfiguration))
                        yield return GetEngine(() => new WinOCREngineWithPreprocess(langDescriptor), detectionLanguage);
                }

                if (confType == typeof(TesseractOCRConfiguration))
                {
                    if (!TryRemoveIfDisabled<TesseractOCREngine>(ocrConfiguration))
                        yield return GetEngine(() => new TesseractOCREngine(langDescriptor), detectionLanguage);

                    if (!TryRemoveIfDisabled<TesseractOCREngineWIthPreprocess>(ocrConfiguration))
                        yield return GetEngine(() => new TesseractOCREngineWIthPreprocess(langDescriptor), detectionLanguage);
                }

                if (confType == typeof(EasyOCRConfiguration))
                {
                    if (!TryRemoveIfDisabled<EasyOCREngine>(ocrConfiguration))
                        yield return GetEngine(() => new EasyOCREngine(langDescriptor, _pythonEngine, _logger), detectionLanguage);
                }
            }

            bool TryRemoveIfDisabled<TEngine>(OcrConfiguration configuration)
                where TEngine : IOCREngine
            {
                if (configuration.Enabled)
                {
                    return false;
                }

                RemoveCachedEngine<TEngine>();

                return true;
            }
        }

        private IOCREngine GetEngine<TEngine>(Func<TEngine> ocrFactoryFunc, Languages detectionLanguage)
            where TEngine : IOCREngine
        {
            var cachedEngine = _cachedEngines.FirstOrDefault(engine => engine.GetType() == typeof(TEngine));
            if (cachedEngine == null)
            {
                cachedEngine = ocrFactoryFunc.Invoke();
                _cachedEngines.Add(cachedEngine);

                return cachedEngine;
            }

            if (cachedEngine.DetectionLanguage == detectionLanguage)
            {
                return cachedEngine;
            }

            //cached engine used another detection language
            RemoveCachedEngine<TEngine>();

            cachedEngine = ocrFactoryFunc.Invoke();
            _cachedEngines.Add(cachedEngine);

            return cachedEngine;
        }


        private void RemoveCachedEngine<TEngine>()
            where TEngine : IOCREngine
        {
            var cachedEngine = _cachedEngines.FirstOrDefault(engine => engine.GetType() == typeof(TEngine));
            if (cachedEngine == null)
            {
                return;
            }

            if (cachedEngine is IDisposable disposableEngine)
            {
                disposableEngine.Dispose();
            }

            _cachedEngines.Remove(cachedEngine);
        }
    }
}
