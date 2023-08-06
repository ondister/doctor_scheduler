using DScheduler.Domain.Rules;

namespace DScheduler.Domain;

public class DoctorStatistics
{
    public DoctorStatistics(
        List<WorkingUnit> units,
        List<ScheduledDay> days,
        List<ScheduledDay> holidays,
        TimeSpan workingDuration,
        IEnumerable< IRuleViolation> violations,
        TimeSpan maxDuration)
    {
        Units = units.AsReadOnly();
        Days = days.AsReadOnly();
        Holidays = holidays.AsReadOnly();
        WorkingDuration = workingDuration;
        Violations = violations;
        MaxDuration = maxDuration;
    }

    public IReadOnlyCollection<WorkingUnit> Units { get; }

    public IReadOnlyCollection<ScheduledDay> Days { get; }

    public IReadOnlyCollection<ScheduledDay> Holidays { get; }

    public TimeSpan WorkingDuration { get; }

    public IEnumerable<IRuleViolation> Violations { get; }

    public TimeSpan MaxDuration { get; }
}