using System.Collections.ObjectModel;

namespace DScheduler.Domain;

public sealed class ScheduledDay : IEquatable<ScheduledDay>
{
    private readonly List<WorkingUnit> _workingUnits;

    private ScheduledDay(DateOnly date, bool isHoliday)
    {
        Date = date;
        IsHoliday = isHoliday;
        _workingUnits = new List<WorkingUnit>();
        WorkingUnits = new ReadOnlyCollection<WorkingUnit>(_workingUnits);
    }

    public ScheduledDay(DateOnly date, bool isHoliday, WorkingUnit workingUnit)
        : this(date, isHoliday)
    {
        AddWorkingUnit(workingUnit);
    }

    public ScheduledDay(DateOnly date, bool isHoliday, IList<WorkingUnit> workingUnits)
        : this(date, isHoliday)
    {
        foreach (var workingUnit in workingUnits)
        {
            AddWorkingUnit(workingUnit);
        }
    }

    public DateOnly Date { get; }

    public bool IsHoliday { get; set; }

    public IReadOnlyList<WorkingUnit> WorkingUnits { get; }

    /// <inheritdoc />
    public bool Equals(ScheduledDay? other)
    {
        if (ReferenceEquals(objA: null, other))
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return Date.Equals(other.Date);
    }

    public void AddWorkingUnit(WorkingUnit workingUnit)
    {
        ArgumentNullException.ThrowIfNull(workingUnit);

        workingUnit.SetScheduledDay(this);
        _workingUnits.Add(workingUnit);
    }

    public ScheduledDayStatictics GetStatistics()
    {
        var hasDutyDoctor = WorkingUnits.OfType<DayWorkingUnit>().Any()
                         && WorkingUnits.OfType<DayWorkingUnit>().All(doctor => doctor != null);

        var doctorsCount = WorkingUnits.Count(doctor => doctor != null);

        return new ScheduledDayStatictics(hasDutyDoctor, doctorsCount);
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || (obj is ScheduledDay other && Equals(other));
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return Date.GetHashCode();
    }

    public static bool operator ==(ScheduledDay? left, ScheduledDay? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(ScheduledDay? left, ScheduledDay? right)
    {
        return !Equals(left, right);
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return Date.ToShortDateString();
    }
}