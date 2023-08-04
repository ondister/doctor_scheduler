using GeneticSharp;

namespace DScheduler.Generation.Genetic;

public sealed class ScheduleChromosome : ChromosomeBase
{
    private readonly int _workingUnitsCount;
    private readonly int _doctorsCount;
    private readonly Random _random;

    /// <inheritdoc />
    public ScheduleChromosome(int workingUnitsCount, int doctorsCount)
        : base(workingUnitsCount)
    {
        if (workingUnitsCount <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(workingUnitsCount),
                message: "Count of working units should be more than 0.");
        }

        if (doctorsCount <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(doctorsCount),
                message: "Count of doctors should be more than 0.");
        }

        _workingUnitsCount = workingUnitsCount;
        _doctorsCount = doctorsCount;
        _random = new Random();

        CreateGenes();
    }

    /// <inheritdoc />
    public override Gene GenerateGene(int geneIndex)
    {
        // TODO: Now genome is 1 day. It should be replaced with workingUnit.
        var genome = new int[1];

        genome[0] = _random.Next(minValue: 0, _doctorsCount);

        return new Gene(genome);
    }

    /// <inheritdoc />
    public override IChromosome CreateNew()
    {
        return new ScheduleChromosome(_workingUnitsCount, _doctorsCount);
    }
}