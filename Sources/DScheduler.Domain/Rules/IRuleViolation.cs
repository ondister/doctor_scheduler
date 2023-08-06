namespace DScheduler.Domain.Rules;

public interface IRuleViolation
{
    string Name { get; }

    string Description { get; }

    double ViolationValue { get; }
}