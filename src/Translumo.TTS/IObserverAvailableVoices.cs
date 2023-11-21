namespace Translumo.TTS;

public interface IObserverAvailableVoices
{
    Task UpdateVoiceAsync(IList<string> currentVoices, CancellationToken token);
}
