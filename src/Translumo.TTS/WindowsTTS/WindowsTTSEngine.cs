using System.Globalization;
using System.Speech.Synthesis;

namespace Translumo.TTS.WindowsTTS;

public class WindowsTTSEngine : ITTSEngine
{
    private SpeechSynthesizer synthesizer;

    public WindowsTTSEngine()
    {
        this.synthesizer = new SpeechSynthesizer();
        this.synthesizer.SetOutputToDefaultAudioDevice();
    }

    public void SpeechText(string text)
    {
        // https://learn.microsoft.com/en-us/archive/msdn-magazine/2019/june/speech-text-to-speech-synthesis-in-net
        
        
        var builder = new PromptBuilder();
        //builder.StartVoice(new CultureInfo("en-US"));
        //builder.AppendText("All we need to do is to keep talking.");
        //builder.EndVoice();
        builder.StartVoice(new CultureInfo("ru-RU"));
        builder.AppendText(text);
        builder.EndVoice();
        this.synthesizer.SpeakAsyncCancelAll();
        this.synthesizer.SpeakAsync(builder);
    }
}