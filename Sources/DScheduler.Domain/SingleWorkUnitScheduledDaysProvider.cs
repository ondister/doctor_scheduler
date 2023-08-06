namespace DScheduler.Domain;

public sealed class SingleWorkUnitScheduledDaysProvider : IScheduledDaysProvider
{
    private readonly DateOnly _startDate;
    private readonly DateOnly _endDate;

    // TODO: Add holidays provider.
    public SingleWorkUnitScheduledDaysProvider(DateOnly startDate, DateOnly endDate)
    {
        _startDate = startDate;
        _endDate = endDate;
    }

    /// <inheritdoc />
    public IEnumerable<ScheduledDay> GetScheduledDays()
    {
        for (var dayIndex = 0; dayIndex <= _endDate.DayNumber - _startDate.DayNumber; dayIndex++)
        {
            var date = _startDate.AddDays(dayIndex);
            var isHoliday = date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday;

            var scheduledDay = new ScheduledDay(date, isHoliday, new DayWorkingUnit(date.ToString(), date));

            yield return scheduledDay;
        }
    }
}