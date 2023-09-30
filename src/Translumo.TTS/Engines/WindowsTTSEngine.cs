using System.Globalization;
using System.Speech.Synthesis;

namespace Translumo.TTS.Engines;

public class WindowsTTSEngine : ITTSEngine
{
    private readonly string _languageCode;
    private readonly SpeechSynthesizer _synthesizer;

    public WindowsTTSEngine(string languageCode)
    {
        _synthesizer = new SpeechSynthesizer();
        _synthesizer.SetOutputToDefaultAudioDevice();
        _languageCode = languageCode;
    }

    public void SpeechText(string text)
    {
        // https://learn.microsoft.com/en-us/archive/msdn-magazine/2019/june/speech-text-to-speech-synthesis-in-net
        var builder = new PromptBuilder();
        builder.StartVoice(new CultureInfo(_languageCode));
        builder.AppendText(text);
        builder.EndVoice();
        _synthesizer.SpeakAsyncCancelAll();
        _synthesizer.SpeakAsync(builder);
    }

    public void Dispose()
    {
        _synthesizer.Dispose();
    }
}