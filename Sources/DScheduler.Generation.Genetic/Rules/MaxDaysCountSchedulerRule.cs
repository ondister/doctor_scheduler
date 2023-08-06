using DScheduler.Domain;
using DScheduler.Domain.Rules;
using DScheduler.Generation.Genetic.Rules;

using GeneticSharp;

namespace DSchedulerGeneration.Genetic.Rules;

public sealed class MaxDaysCountSchedulerRule : BaseGeneticSchedulerRule
{
    private readonly Scheduler _scheduler;
    private readonly IDictionary<int,double> _maxDoctorsUnitsCount;

    /// <inheritdoc />
    public MaxDaysCountSchedulerRule(double weight, Scheduler scheduler)
        : base(weight)
    {
        _scheduler = scheduler;
        _maxDoctorsUnitsCount = new Dictionary<int, double>();
        var workingDays = scheduler.SchedulerdDays.Count(day => !day.IsHoliday);

        for (var doctorIndex = 0; doctorIndex < scheduler.Doctors.Count; doctorIndex++)
        {
           if(scheduler.Doctors[doctorIndex].WorkingDayDuration!=TimeSpan.Zero)
            {
                _maxDoctorsUnitsCount.Add(doctorIndex, workingDays * scheduler.Doctors[doctorIndex].WorkingDayDuration.TotalMinutes);
            }
        }

    }

    /// <inheritdoc />
    protected override double CalculateInternal(Gene[] genes)
    {
        var result = 0.0;

        var realWorkingLoading = new Dictionary<int, double>();

        foreach (var kvp in _maxDoctorsUnitsCount)
        {
            realWorkingLoading.Add(kvp.Key, 0.0);
        }

        for (var dayIndex = 0; dayIndex < genes.Length; dayIndex++)
        {

                var todayDoctorIndex = ((int[])genes[dayIndex].Value)[0];

                if (!realWorkingLoading.ContainsKey(todayDoctorIndex))
                {
                    continue;
                }
                    foreach (var unit in _scheduler.SchedulerdDays[dayIndex].WorkingUnits)
                    {
                        realWorkingLoading[todayDoctorIndex] += unit.Duration.TotalMinutes;
                    }

        }

        foreach (var kvp in _maxDoctorsUnitsCount)
        {
            if (Math.Abs(kvp.Value - realWorkingLoading[kvp.Key])>TimeSpan.FromHours(12).TotalMinutes)
            {
                result++;
            }
        }
       

        return result * Weight;
    }

    /// <inheritdoc />
    protected override IRuleViolation GetViolationInternal(Scheduler scheduler, Doctor doctor)
    {
        if (doctor.WorkingDayDuration == TimeSpan.Zero)
        {
            return new ValueDifferenceViolation(GetType().Name, 0);
        }

        var workingDuration = TimeSpan.FromMinutes(
            _scheduler.SchedulerdDays
                .SelectMany(day => day.WorkingUnits)
                .Where(unit => unit.Doctor == doctor)
                .Sum(d => d.Duration.TotalMinutes));

        var workingDays = scheduler.SchedulerdDays.Count(day => !day.IsHoliday);
        var maxWorkingTime = workingDays * doctor.WorkingDayDuration.TotalMinutes;

        return new ValueDifferenceViolation(GetType().Name, maxWorkingTime-workingDuration.TotalMinutes);
    }
}