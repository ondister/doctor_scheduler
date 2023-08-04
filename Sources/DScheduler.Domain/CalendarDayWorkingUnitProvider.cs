namespace DScheduler.Domain;

public sealed class CalendarDayWorkingUnitProvider : IWorkUnitProvider
{
    private readonly DateOnly _startDate;
    private readonly DateOnly _endDate;

    public CalendarDayWorkingUnitProvider(DateOnly startDate, DateOnly endDate)
    {
        _startDate = startDate;
        _endDate = endDate;
    }

    /// <inheritdoc />
    public IEnumerable<WorkingUnit> GetWorkingUnits()
    {
        var workingUnits = new List<DayWorkingUnit>();
        for (var dayIndex = 0; dayIndex <= _endDate.DayNumber - _startDate.DayNumber; dayIndex++)
        {
            var date = _startDate.AddDays(dayIndex);
            var dayWorkingUnit = new DayWorkingUnit(date.ToString(), date)
                { IsHoliday = date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday };

            workingUnits.Add(dayWorkingUnit);
        }

        return workingUnits;
    }
}