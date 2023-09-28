using System.Globalization;
using System.Speech.Synthesis;

namespace Translumo.TTS.Engines;

public class WindowsTTSEngine : ITTSEngine
{
    private readonly string languageCode;
    private SpeechSynthesizer synthesizer;

    public WindowsTTSEngine(string languageCode)
    {
        synthesizer = new SpeechSynthesizer();
        synthesizer.SetOutputToDefaultAudioDevice();
        this.languageCode = languageCode;
    }

    public void SpeechText(string text)
    {
        // https://learn.microsoft.com/en-us/archive/msdn-magazine/2019/june/speech-text-to-speech-synthesis-in-net
        var builder = new PromptBuilder();
        builder.StartVoice(new CultureInfo(languageCode));
        builder.AppendText(text);
        builder.EndVoice();
        synthesizer.SpeakAsyncCancelAll();
        synthesizer.SpeakAsync(builder);
    }
}