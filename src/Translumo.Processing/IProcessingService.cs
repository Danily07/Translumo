namespace Translumo.Processing
{
    public interface IProcessingService
    {
        bool IsStarted { get; }

        void StartProcessing();

        void StopProcessing();
    }
}
