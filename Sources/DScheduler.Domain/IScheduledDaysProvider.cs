namespace DScheduler.Domain;

public interface IScheduledDaysProvider
{
    IEnumerable<ScheduledDay> GetScheduledDays();
}