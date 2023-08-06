namespace DScheduler.Domain.Generation
{
    public interface ISchedulerGenerationStrategy
    {
        Task<IList<ScheduledDay>> GenerateAsync(GenerationSource source, IProgress<GenerationProgressInfo> progress, CancellationToken cancellationToken);
    }
}