namespace Translumo.TTS.Engines;

public interface ITTSEngine : IDisposable
{
    void SpeechText(string text);

    string[] GetVoices();

    void SetVoice(string voice);
}
