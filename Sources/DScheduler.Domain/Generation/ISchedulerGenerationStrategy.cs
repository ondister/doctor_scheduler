namespace DScheduler.Domain.Generation
{
    public interface ISchedulerGenerationStrategy
    {
        Task<IDictionary<WorkingUnit, Doctor>> GenerateAsync(GenerationSource source, IProgress<GenerationProgressInfo> progress, CancellationToken cancellationToken);
    }
}