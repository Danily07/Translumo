using Python.Runtime;
using System;
using System.Media;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static Tensorboard.Summary.Types;
//using TorchSharp;
//using static TorchSharp.torch;
//using static TorchSharp.torch.nn;

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


while (true)
{
    Console.WriteLine("Напиши текст а я скажу:");
    var text = Console.ReadLine();
    dynamic result;
    using (Py.GIL())
    {
        dynamic audio = model.apply_tts(text: text,
                         speaker: "baya",
                         sample_rate: 48000);
        result = ipython.display.Audio(audio, rate: 48000).data;
    }

    using (MemoryStream ms = new MemoryStream(result.As<byte[]>()))
    {
        // Construct the sound player
        var player = new SoundPlayer(ms);
        player.Play();
    }

}



