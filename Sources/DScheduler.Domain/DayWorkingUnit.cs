namespace DScheduler.Domain;

public sealed class DayWorkingUnit : WorkingUnit
{
    /// <inheritdoc />
    public DayWorkingUnit(string name, DateOnly date)
        : base(name)
    {
        Date = date;
    }

    public DateOnly Date { get; }

    /// <inheritdoc />
    public override TimeSpan Duration { get; } = TimeSpan.FromHours(24);
}