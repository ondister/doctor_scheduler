﻿using DScheduler.Domain;
using DScheduler.Domain.Rules;
using DScheduler.Generation.Genetic;
using DScheduler.Generation.Genetic.Rules;

using GeneticSharp;

namespace DSchedulerGeneration.Genetic.Rules;

public sealed class DaysDispersionSchedulerRule : BaseGeneticSchedulerRule
{
    private readonly int _doctorsCount;

    /// <inheritdoc />
    public DaysDispersionSchedulerRule(double weight, int doctorsCount)
        : base(weight)
    {
        if (doctorsCount <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(doctorsCount), message: "Doctors count should be more than 0");
        }

        _doctorsCount = doctorsCount;
    }

    /// <inheritdoc />
    protected override double CalculateInternal(Gene[] genes)
    {
        var result = 0.0;

        var shiftCounts = new double[_doctorsCount];

        for (var doctorIndex = 0; doctorIndex < _doctorsCount; doctorIndex++)
        {
            double shiftsCount = genes.Count(v => ((int[])v.Value)[0] == doctorIndex);
            shiftCounts[doctorIndex] = shiftsCount;
        }

        result = Mathematics.CalculateDispersion(shiftCounts);
        return result * Weight;
    }

    /// <inheritdoc />
    protected override IRuleViolation GetViolationInternal(Scheduler scheduler, Doctor doctor)
    {
        if (scheduler.Doctors.Count == 0)
        {
            return new ValueDifferenceViolation(GetType().Name, 0);
        }
        var expectedDaysCount = scheduler.SchedulerdDays.Count / scheduler.Doctors.Count;
        var doctorDaysCount = scheduler.SchedulerdDays
                                       .Count(
                                           day => day.WorkingUnits
                                                     .Any(unit => unit.Doctor == doctor));

        return new ValueDifferenceViolation(GetType().Name, expectedDaysCount - doctorDaysCount);
    }
}