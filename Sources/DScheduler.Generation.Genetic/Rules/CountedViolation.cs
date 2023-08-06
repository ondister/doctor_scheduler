using DScheduler.Domain;
using DScheduler.Domain.Rules;

namespace DScheduler.Generation.Genetic.Rules;

public sealed class CountedViolation : IRuleViolation
{
    public CountedViolation(string ruleName, double violationsCount, IEnumerable<WorkingUnit> workingUnits)
    {
        RuleName = ruleName;
        ViolationValue = violationsCount;
        WorkingUnits = workingUnits;
    }

    /// <inheritdoc />
    public string Name { get; } = "Counted violation";

    /// <inheritdoc />
    public string Description { get; } = "Count of a rule violations";

    public string RuleName { get; }

    /// <inheritdoc />
    public double ViolationValue { get; }

    public IEnumerable<WorkingUnit> WorkingUnits { get; }

    /// <inheritdoc />
    public override string ToString()
    {
        return $"{RuleName} - Violations count: {ViolationValue} \n Units: {string.Join("\n",WorkingUnits)}";
    }
}