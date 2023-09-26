using System.Globalization;
using System.Speech.Synthesis;

namespace Translumo.TTS.WindowsTTS;

public class WindowsTTSEngine : ITTSEngine
{
    public void SpeechText(string text)
    {
        // https://learn.microsoft.com/en-us/archive/msdn-magazine/2019/june/speech-text-to-speech-synthesis-in-net
        var synthesizer = new SpeechSynthesizer();
        synthesizer.SetOutputToDefaultAudioDevice();
        var builder = new PromptBuilder();
        //builder.StartVoice(new CultureInfo("en-US"));
        //builder.AppendText("All we need to do is to keep talking.");
        //builder.EndVoice();
        builder.StartVoice(new CultureInfo("ru-RU"));
        builder.AppendText(text);
        builder.EndVoice();
        synthesizer.Speak(builder);
    }
}