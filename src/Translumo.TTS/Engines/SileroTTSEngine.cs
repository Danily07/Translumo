using Python.Runtime;
using System.Media;
using Translumo.Infrastructure.Constants;

namespace Translumo.TTS.Engines;

public class SileroTTSEngine : ITTSEngine
{
    private dynamic _ipython;
    private dynamic _model;
    private string _voice;
    private List<PyObject> _pyObjects = new();
    private object _syncContext = new object();
    private bool _disposed = true;
    private readonly SoundPlayer _player = new SoundPlayer();

    private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

    private const int _wave_rate = 48000;


    public SileroTTSEngine()
    {
        Init();
    }

    private void Init()
    {
        if (!PythonEngine.IsInitialized)
        {
            // TODO: move to common place, also used in EasyOCR
            Runtime.PythonDLL = Path.Combine(Global.PythonPath, "python38.dll");
            PythonEngine.Initialize();
            PythonEngine.BeginAllowThreads();
        }

        using (Py.GIL())
        {
            var np = Py.Import("numpy");
            _ipython = Py.Import("IPython");
            dynamic torch = Py.Import("torch");

            using dynamic device = torch.device("cpu");
            torch.set_num_threads(4);

            _model = torch.package.PackageImporter("v4_ru.pt").load_pickle("tts_models", "model");
            _model.to(device);

            _pyObjects.Add(_model);
            _pyObjects.Add(np);
            _pyObjects.Add(torch);
            _pyObjects.Add(_ipython);
        }

        using PyObject speakers = _model.speakers;
        _voice = speakers.As<string[]>().First();
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

    private byte[] GenerateAudio(string text)
    {
        byte[] result;
        using (Py.GIL())
        {
            using PyObject audio = _model.apply_tts(text: text, speaker: _voice, sample_rate: _wave_rate);
            using PyObject pyAudio = _ipython.display.Audio(audio, rate: _wave_rate);
            result = ((pyAudio as dynamic).data as PyObject).As<byte[]>();
        }

        return result;
    }

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

        if (PythonEngine.IsInitialized)
        {
            //Causes PythonEngine.Shutdown() hanging (https://github.com/pythonnet/pythonnet/issues/1701)
            //PythonEngine.EndAllowThreads(_threadState);
            PythonEngine.Shutdown();
        }
    }
}