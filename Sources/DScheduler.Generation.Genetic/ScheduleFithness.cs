using DScheduler.Domain.Rules;
using DScheduler.Generation.Genetic.Rules;

using GeneticSharp;

namespace DScheduler.Generation.Genetic;

public sealed class ScheduleFithness : IFitness
{
    private readonly IEnumerable<BaseGeneticSchedulerRule> _rules;

    public ScheduleFithness(IEnumerable<ISchedulerRule> rules)
    {
        _rules = rules.OfType<BaseGeneticSchedulerRule>();
    }

    /// <inheritdoc />
    public double Evaluate(IChromosome chromosome)
    {
        var genes = chromosome.GetGenes();

        var result = 0.0;

        foreach (var rule in _rules)
        {
            rule.Genes = genes;

            result += rule.Calculate();
        }

        return result * -1;
    }
}