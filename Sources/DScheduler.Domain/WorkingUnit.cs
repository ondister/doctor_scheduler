namespace DScheduler.Domain;

public abstract class WorkingUnit
{
    protected WorkingUnit(string name)
    {
        Name = name;
    }

    public string Name { get; }

    public Doctor? Doctor { get; private set; }

    public ScheduledDay? ScheduledDay { get; private set; }

    public void SetDoctor(Doctor doctor)
    {
        ArgumentNullException.ThrowIfNull(doctor);

        Doctor = doctor;
    }

    public void SetScheduledDay(ScheduledDay scheduledDay)
    {
        ArgumentNullException.ThrowIfNull(scheduledDay);

        ScheduledDay = scheduledDay;
    }

    public abstract TimeSpan Duration { get; }

    /// <inheritdoc />
    public override string ToString()
    {
        return Name;
    }
}