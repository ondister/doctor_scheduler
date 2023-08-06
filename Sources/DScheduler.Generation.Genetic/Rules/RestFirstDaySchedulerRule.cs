﻿using DScheduler.Domain;
using DScheduler.Domain.Rules;
using DScheduler.Generation.Genetic.Rules;

using GeneticSharp;

namespace DSchedulerGeneration.Genetic.Rules;

public sealed class RestFirstDaySchedulerRule : BaseGeneticSchedulerRule
{
    /// <inheritdoc />
    public RestFirstDaySchedulerRule(double weight)
        : base(weight) { }

    /// <inheritdoc />
    protected override double CalculateInternal(Gene[] genes)
    {
        var result = 0.0;

        for (var dayIndex = 0; dayIndex < genes.Length; dayIndex++)
        {
            var tomorrowIndex = dayIndex + 1;

            var tomorrowDoctorIndex = -1;

            var todayDoctorIndex = ((int[])genes[dayIndex].Value)[0];

            if (tomorrowIndex < genes.Length)
            {
                tomorrowDoctorIndex = ((int[])genes[tomorrowIndex].Value)[0];
            }

            if (todayDoctorIndex == tomorrowDoctorIndex)
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

        for (var dayIndex = 0; dayIndex < scheduler.SchedulerdDays.Count; dayIndex++)
        {
            var tomorrowIndex = dayIndex + 1;

            for (var unitIndex = 0; unitIndex < scheduler.SchedulerdDays[dayIndex].WorkingUnits.Count; unitIndex++)
            {
                if (scheduler.SchedulerdDays[dayIndex].WorkingUnits[index: 0].Doctor == doctor
                 && tomorrowIndex < scheduler.SchedulerdDays.Count)
                {
                    var afterTomorrowDoctor =
                        scheduler.SchedulerdDays[tomorrowIndex].WorkingUnits[index: 0].Doctor;
                    if (afterTomorrowDoctor == doctor)
                    {
                        violationUnits.Add(scheduler.SchedulerdDays[tomorrowIndex].WorkingUnits[index: 0]);
                    }
                }
            }
        }

        return new CountedViolation(GetType().Name, violationUnits.Count, violationUnits);
    }
}