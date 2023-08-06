namespace DScheduler.Domain;

public sealed class Doctor : IEquatable<Doctor>
{
    private readonly List<DateOnly> _expectedDays;
    private readonly List<DateOnly> _wishDays;

    public Doctor(string name, bool isVip = false)
    {
        Name = name;
        IsVip = isVip;
        _expectedDays = new List<DateOnly>();
        _wishDays = new List<DateOnly>();
    }

    public IReadOnlyList<DateOnly> ExpectedDays => _expectedDays.AsReadOnly();

    public IReadOnlyList<DateOnly> WishDays => _wishDays.AsReadOnly();

    public string Name { get; }

    public bool IsVip { get; }

    public TimeSpan WorkingDayDuration { get; private set; }

    public Doctor AddExpectedDay(DateOnly day)
    {
        if (_expectedDays.Contains(day))
        {
            return this;
        }

        _expectedDays.Add(day);

        return this;
    }

    public Doctor SetWorkingDayDuration(TimeSpan workingDayDuration)
    {
       WorkingDayDuration= workingDayDuration;

       return this;
    }

    /// <inheritdoc />
    public bool Equals(Doctor? other)
    {
        if (ReferenceEquals(objA: null, other))
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return Name == other.Name;
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || (obj is Doctor other && Equals(other));
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return Name.GetHashCode();
    }

    public static bool operator ==(Doctor? left, Doctor? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(Doctor? left, Doctor? right)
    {
        return !Equals(left, right);
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return Name;
    }
}