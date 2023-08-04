using DScheduler.Generation.Genetic.Rules;

using GeneticSharp;

namespace DSchedulerGeneration.Genetic.Rules;

public sealed class DaysDispersionSchedulerRule : BaseGeneticSchedulerRule
{
    private readonly int _doctorsCount;

    /// <inheritdoc />
    public DaysDispersionSchedulerRule(double weight, int doctorsCount)
        : base(weight)
    {
        if (doctorsCount <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(doctorsCount), message: "Doctors count should be more than 0");
        }

        _doctorsCount = doctorsCount;
    }

    /// <inheritdoc />
    protected override double CalculateInternal(Gene[] genes)
    {
        var result = 0.0;

        double expectedAverage = genes.Length / _doctorsCount;

        var shiftCounts = new double[_doctorsCount];

        for (var doctorIndex = 0; doctorIndex < _doctorsCount; doctorIndex++)
        {
            double shiftsCount = genes.Count(v => ((int[])v.Value)[0] == doctorIndex);
            shiftCounts[doctorIndex] = shiftsCount;
        }

        result = CalculateDispersion(shiftCounts);
        return result * 0.5;
    }

    private static double CalculateDispersion(double[] shiftCounts)
    {
        var average = shiftCounts.Average();
        var sumOfSquaresOfDifferences = shiftCounts.Select(value => (value - average) * (value - average)).Sum();
        var dispersion = Math.Sqrt(sumOfSquaresOfDifferences / shiftCounts.Length);

        return dispersion;
    }
}