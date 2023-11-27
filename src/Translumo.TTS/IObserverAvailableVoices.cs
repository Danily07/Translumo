namespace Translumo.TTS;

public interface IObserverAvailableVoices
{
    void UpdateVoice(IList<string> currentVoices);
}
