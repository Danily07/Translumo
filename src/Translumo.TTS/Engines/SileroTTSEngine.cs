using System.Media;
using Translumo.Infrastructure.Python;

namespace Translumo.TTS.Engines;

public class SileroTTSEngine : ITTSEngine
{
    private dynamic _ipython;
    private dynamic _model;
    private string _voice;
    private List<IDisposable> _pyObjects = new();
    private object _syncContext = new object();
    private bool _disposed = true;
    private readonly SoundPlayer _player = new SoundPlayer();
    private readonly PythonEngineWrapper _pythonEngine;
    private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

    private const int _wave_rate = 48000;


    public SileroTTSEngine(PythonEngineWrapper pythonEngine)
    {
        _pythonEngine = pythonEngine;
        Init();
    }

    private void Init()
    {
        _pythonEngine.Init();

        _pythonEngine.Execute(() =>
        {
            var np = _pythonEngine.Import("numpy");
            _ipython = _pythonEngine.Import("IPython");
            dynamic torch = _pythonEngine.Import("torch");

            using dynamic device = torch.device("cpu");
            torch.set_num_threads(4);

            _model = torch.package.PackageImporter("v4_ru.pt").load_pickle("tts_models", "model");
            _model.to(device);

            _pyObjects.Add(_model);
            _pyObjects.Add(np);
            _pyObjects.Add(torch);
            _pyObjects.Add(_ipython);
        });

        _voice = ((string[])_model.speakers).First();
    }

    public void SpeechText(string text)
    {
        _cancellationTokenSource.Cancel();
        _cancellationTokenSource = new CancellationTokenSource();

        Task.Factory.StartNew(() => GenerateAudio(text), _cancellationTokenSource.Token)
            .ContinueWith((bytesTask) =>
             {
                 if (!bytesTask.IsCompletedSuccessfully)
                 {
                     return;
                 }

                 PlayWavBytes(bytesTask.Result);
             }, _cancellationTokenSource.Token);
    }

    private byte[] GenerateAudio(string text) =>
        _pythonEngine.Execute(() =>
        {
            using var audio = _model.apply_tts(text: text, speaker: _voice, sample_rate: _wave_rate);
            using var pyAudio = _ipython.display.Audio(audio, rate: _wave_rate);
            return (byte[])pyAudio.data;
        });

    private void PlayWavBytes(byte[] wavBytes)
    {
        _player.Stop();
        using (var ms = new MemoryStream(wavBytes))
        {
            _player.Stream = ms;
            _player.Play();
        }
    }

    public void Dispose()
    {
        _cancellationTokenSource.Cancel();
        _cancellationTokenSource.Dispose();
        _player.Stop();
        _player.Dispose();

        foreach (var pyObject in _pyObjects)
        {
            pyObject.Dispose();
        }

        _pythonEngine.Dispose();
    }
}