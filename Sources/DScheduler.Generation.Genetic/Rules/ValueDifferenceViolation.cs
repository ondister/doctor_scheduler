using DScheduler.Domain.Rules;

namespace DScheduler.Generation.Genetic.Rules;

public sealed class ValueDifferenceViolation : IRuleViolation
{
    public ValueDifferenceViolation(string ruleName, double valueDifference)

    {
        RuleName = ruleName;
        ViolationValue = valueDifference;
    }

    /// <inheritdoc />
    public string Name { get; } = "Value difference violation";

    /// <inheritdoc />
    public string Description { get; } = "Measure of value difference";

    public string RuleName { get; }

    /// <inheritdoc />
    public double ViolationValue { get; }

    /// <inheritdoc />
    public override string ToString()
    {
        return $"{RuleName} - Value diff: {ViolationValue}";
    }
}