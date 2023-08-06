namespace DScheduler.Domain.Generation;

public sealed class GenerationProgressInfo
{
    public GenerationProgressInfo(int generationsNumber, TimeSpan timeEvolving, GenerationStatus generationStatus)
    {
        GenerationsNumber = generationsNumber;
        TimeEvolving = timeEvolving;
        GenerationSttus = generationStatus;
    }

    public int GenerationsNumber { get; }

    public TimeSpan TimeEvolving { get; }

    public GenerationStatus GenerationSttus { get; }
}