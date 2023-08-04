using DScheduler.Domain;

using GeneticSharp;

namespace DScheduler.Generation.Genetic.Rules;

public abstract class BaseGeneticSchedulerRule : ISchedulerRule
{
    protected BaseGeneticSchedulerRule(double weight)
    {
        if (weight < 0 || weight > 10)
        {
            throw new ArgumentOutOfRangeException(nameof(weight), message: "Rule weight should be from 0 to 10");
        }

        Weight = weight;
    }

    /// <inheritdoc />
    public double Weight { get; }

    internal Gene[]? Genes { get; set; }

    /// <inheritdoc />
    public double Calculate()
    {
        if (Genes == null)
        {
            return 0;
        }

        return CalculateInternal(Genes);
    }

    protected abstract double CalculateInternal(Gene[] genes);
}