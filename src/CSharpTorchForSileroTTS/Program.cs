using Python.Runtime;
using System.Media;

// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");


var pythonPath = @"C:\Users\Ivan\source\repos\Translumo\src\Translumo\bin\Debug\net7.0-windows10.0.19041.0\win10-x64\python";
Runtime.PythonDLL = Path.Combine(pythonPath, "python38.dll");
PythonEngine.PythonHome = pythonPath;

if (!PythonEngine.IsInitialized)
{
    PythonEngine.Initialize();
    PythonEngine.BeginAllowThreads();
}
dynamic ipython;
dynamic model;
using (Py.GIL())
{
    //dynamic io = Py.Import("io");
    //Py.Import("ffmpegio");   
    //dynamic ipython = Py.Import("ipykernel");
    dynamic np = Py.Import("numpy");
    ipython = Py.Import("IPython");
    // dynamic torchaudio = Py.Import("torchaudio");
    dynamic torch = Py.Import("torch");

    dynamic device = torch.device("cpu");
    torch.set_num_threads(4);
    model = torch.package.PackageImporter("v4_ru.pt").load_pickle("tts_models", "model");
    model.to(device);
}

var voice = GetVoices(model).First();

//////////
SoundPlayer? _player = null;
var _cancellationTokenSource = new CancellationTokenSource();
////////

SpeechText("Халлоу");
SpeechText("Привет");


while (true)
{
    Console.WriteLine("Напиши текст а я скажу:");
    var text = Console.ReadLine();

    if (string.IsNullOrEmpty(text))
    {
        voice = ChangeVoice(model);
        continue;
    }

    SpeechText(text);
}


string ChangeVoice(dynamic model)
{
    var result = "";
    var voices = GetVoices(model);
    do
    {
        Console.WriteLine("Please choose voice:");
        voices.Select((x, i) => (i + 1, x)).ToList().ForEach(x => Console.WriteLine($"{x.Item1}. {x.Item2}"));
        var answer = Console.ReadLine();
        if (int.TryParse(answer, out var choosesVariant)
            && choosesVariant <= voices.Length
            && choosesVariant > 0)
        {
            result = voices[choosesVariant - 1];
        }

    } while (string.IsNullOrEmpty(result));

    return result;
}

string[] GetVoices(dynamic model)
{
    return model.speakers.As<string[]>();
}




void SpeechText(string text)
{

    StopSpeech();

    //var token = _cancellationTokenSource.Token;
    //var task = Task.Factory.StartNew(
    //    () =>
    //    {

    //        var bytes = GenerateAudio(text);
    //        if (token.IsCancellationRequested)
    //        {
    //            Console.Write(",");
    //            return;
    //        }
    //        PlayWavBytes(bytes);
    //    });

    var task = Task.Factory.StartNew(() => GenerateAudio(text), _cancellationTokenSource.Token)
    .ContinueWith((bytesTask) => {
        if (!bytesTask.IsCompletedSuccessfully)
        {
            return;
        }
        PlayWavBytes(bytesTask.Result);
    }, _cancellationTokenSource.Token);
}

void StopSpeech()
{
    _cancellationTokenSource.Cancel();
    _player?.Stop();
    _cancellationTokenSource.Dispose();    
    _cancellationTokenSource = new CancellationTokenSource();
}

byte[] GenerateAudio(string text)
{
    Console.Write("`");
    byte[] result;
    using (Py.GIL())
    {
        using PyObject audio = model.apply_tts(text: text, speaker: voice, sample_rate: 48000);
        using PyObject pyAudio = ipython.display.Audio(audio, rate: 48000);
        result = ((pyAudio as dynamic).data as PyObject).As<byte[]>();
    }
    Console.Write("'");
    return result;
}

void PlayWavBytes(byte[] wavBytes)
{
    using var ms = new MemoryStream(wavBytes);
    using var player = new SoundPlayer(ms);
    _player = player;
    player.Play();
    //.player.Dispose();
    _player = null;
}
