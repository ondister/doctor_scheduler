using DScheduler.Domain;
using DScheduler.Domain.Rules;

using GeneticSharp;

namespace DScheduler.Generation.Genetic.Rules;

public abstract class BaseGeneticSchedulerRule : ISchedulerRule
{
    protected BaseGeneticSchedulerRule(double weight)
    {
        if (weight < 0 || weight > 20)
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

    /// <inheritdoc />
    public IRuleViolation GetViolationForDoctor(Scheduler scheduler, Doctor doctor)
    {
        return GetViolationInternal(scheduler, doctor);
    }

    protected abstract double CalculateInternal(Gene[] genes);

    protected abstract IRuleViolation GetViolationInternal(Scheduler scheduler, Doctor doctor);
}