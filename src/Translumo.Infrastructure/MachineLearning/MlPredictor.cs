using System;
using System.IO;
using Microsoft.ML;

namespace Translumo.Infrastructure.MachineLearning
{
    public class MlPredictor<TInput, TOutput> : IPredictor<TInput, TOutput>
        where TInput: class
        where TOutput: class, new()
    {
        public bool Loaded => _model != null;

        private readonly MLContext _context;
        private ITransformer _model;
        private PredictionEngine<TInput, TOutput> _predictor;

        public MlPredictor()
        {
            this._context = new MLContext();
        }

        public void LoadModel(string path)
        {
            if (!File.Exists(path))
            {
                throw new ArgumentException($"Model '{path}' is not found");
            }
            _predictor?.Dispose();
            
            _model = _context.Model.Load(path, out var schema);
            _predictor = _context.Model.CreatePredictionEngine<TInput, TOutput>(_model);
        }

        public void UnloadModel()
        {
            _predictor?.Dispose();
            _model = null;
            _predictor = null;
        }

        public TOutput PredictResult(TInput input)
        {
            if (!Loaded)
            {
                throw new InvalidOperationException("Model is not loaded");
            }

            return _predictor.Predict(input);
        }

        public void Dispose()
        {
            _predictor?.Dispose();
        }
    }
}
