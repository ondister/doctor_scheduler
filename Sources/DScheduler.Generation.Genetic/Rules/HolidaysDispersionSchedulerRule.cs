using DScheduler.Domain;
using DScheduler.Domain.Rules;
using DScheduler.Generation.Genetic;
using DScheduler.Generation.Genetic.Rules;

using GeneticSharp;

namespace DSchedulerGeneration.Genetic.Rules;

public sealed class HolidaysDispersionSchedulerRule : BaseGeneticSchedulerRule
{
    private readonly int _doctorsCount;
    private readonly HashSet<int> _holidaysIndices;

    /// <inheritdoc />
    public HolidaysDispersionSchedulerRule(double weight, int doctorsCount, HashSet<int> holidaysIndices)
        : base(weight)
    {
        if (doctorsCount <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(doctorsCount), message: "Doctors count should be more than 0");
        }

        _doctorsCount = doctorsCount;
        _holidaysIndices = holidaysIndices;
    }

    /// <inheritdoc />
    protected override double CalculateInternal(Gene[] genes)
    {
        var result = 0.0;

        if (_holidaysIndices == null || _holidaysIndices.Count == 0)
        {
            return result;
        }

        var shiftCounts = new double[_doctorsCount];
        var genesOfHolidays = new List<Gene>();

        for (var dayIndex = 0; dayIndex < genes.Length; dayIndex++)
        {
            if (_holidaysIndices.Contains(dayIndex))
            {
                genesOfHolidays.Add(genes[dayIndex]);
            }
        }

        for (var doctorIndex = 0; doctorIndex < _doctorsCount; doctorIndex++)
        {
            double shiftsCount = genesOfHolidays.Count(v => ((int[])v.Value)[0] == doctorIndex);
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
        var expectedDaysCount = scheduler.SchedulerdDays.Count(day=>day.IsHoliday) / scheduler.Doctors.Count;
        var doctorDaysCount = scheduler.SchedulerdDays
                              .Where(day => day.IsHoliday)
                              .Count(day => day.WorkingUnits.Any(unit => unit.Doctor == doctor));

        return new ValueDifferenceViolation(GetType().Name, expectedDaysCount - doctorDaysCount);
    }
}