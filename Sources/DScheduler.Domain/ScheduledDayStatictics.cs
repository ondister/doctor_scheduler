namespace DScheduler.Domain;

public class ScheduledDayStatictics
{
    public ScheduledDayStatictics(bool hasDutyDoctor, int doctorsCount)
    {
        HasDutyDoctor = hasDutyDoctor;
        DoctorsCount = doctorsCount;
    }

    public bool HasDutyDoctor { get; }

    public int DoctorsCount { get; }
}