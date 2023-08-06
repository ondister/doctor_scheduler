using DScheduler.Domain;
using DScheduler.Domain.Rules;
using DScheduler.Generation.Genetic.Rules;

using GeneticSharp;

namespace DSchedulerGeneration.Genetic.Rules;

public sealed class DaysExceptionSchedulerRule : BaseGeneticSchedulerRule
{
    private readonly IDictionary<int, int[]> _doctorsExpectedDays;

    /// <inheritdoc />
    public DaysExceptionSchedulerRule(double weight, IDictionary<int, int[]> doctorsExpectedDays)
        : base(weight)
    {
        _doctorsExpectedDays = doctorsExpectedDays;
    }

    /// <inheritdoc />
    protected override double CalculateInternal(Gene[] genes)
    {
        var result = 0.0;

        for (var dayIndex = 0; dayIndex < genes.Length; dayIndex++)
        {
            var todayDoctorIndex = ((int[])genes[dayIndex].Value)[0];

            if (_doctorsExpectedDays.TryGetValue(todayDoctorIndex, out var days) && days.Contains(dayIndex))
            {
                result++;
            }
        }

        return result * Weight;
    }

    /// <inheritdoc />
    protected override IRuleViolation GetViolationInternal(Scheduler scheduler, Doctor doctor)
    {
        var violationUnits = new List<WorkingUnit>();

        foreach (var expectedDay in doctor.ExpectedDays)
        {
            var units = scheduler.SchedulerdDays.SelectMany(day => day.WorkingUnits)
                                        .Where(unit => unit.ScheduledDay is not null && unit.Doctor == doctor && unit.ScheduledDay.Date == expectedDay);

            foreach (var unit in units)
            {
                violationUnits.Add(unit);
            }
        }

        return new CountedViolation(GetType().Name, violationUnits.Count, violationUnits);
    }
}