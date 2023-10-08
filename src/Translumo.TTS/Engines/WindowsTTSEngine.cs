using System.Collections.ObjectModel;
using System.Globalization;
using System.Speech.Synthesis;

namespace Translumo.TTS.Engines;

public class WindowsTTSEngine : ITTSEngine
{
    private VoiceInfo _voiceInfo;
    private readonly SpeechSynthesizer _synthesizer;
    private readonly ReadOnlyDictionary<string, VoiceInfo> _voices;

    public WindowsTTSEngine(string languageCode)
    {
        _synthesizer = new SpeechSynthesizer();
        _synthesizer.SetOutputToDefaultAudioDevice();
        _synthesizer.Rate = 1;
        _voices = _synthesizer.GetInstalledVoices(new CultureInfo(languageCode)).ToDictionary(x => x.VoiceInfo.Name, x => x.VoiceInfo).AsReadOnly();
        _voiceInfo = _voices.First().Value;
    }

    public void SpeechText(string text)
    {
        // https://learn.microsoft.com/en-us/archive/msdn-magazine/2019/june/speech-text-to-speech-synthesis-in-net
        if (_voiceInfo == null)
        {
            return;
        }
        var builder = new PromptBuilder();
        builder.StartVoice(_voiceInfo);
        builder.AppendText(text);
        builder.EndVoice();
        _synthesizer.SpeakAsyncCancelAll();
        _synthesizer.SpeakAsync(builder);
    }

    public void Dispose()
    {
        _synthesizer.Dispose();
    }

    public string[] GetVoices() => _voices.Keys.ToArray();

    public void SetVoice(string voice) => _voiceInfo = _voices.First(x => x.Key.Equals(voice, StringComparison.OrdinalIgnoreCase)).Value;
}