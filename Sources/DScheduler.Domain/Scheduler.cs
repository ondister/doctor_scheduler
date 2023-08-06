using System.Collections.ObjectModel;

using DScheduler.Domain.Generation;
using DScheduler.Domain.Rules;

namespace DScheduler.Domain;

public sealed class Scheduler
{
    private readonly IScheduledDaysProvider _scheduledDaysProvider;
    private readonly ISchedulerGenerationStrategy _schedulerGenerationStrategy;
    private readonly IList<ISchedulerRule> _rules;
    private readonly IList<Doctor> _doctors;
    private readonly IList<ScheduledDay> _scheduledDays;

    public Scheduler(
        IScheduledDaysProvider scheduledDaysProvider,
        ISchedulerGenerationStrategy schedulerGenerationStrategy)
    {
        _scheduledDaysProvider = scheduledDaysProvider;
        _schedulerGenerationStrategy = schedulerGenerationStrategy;
        _scheduledDays = new List<ScheduledDay>();
        _rules = new List<ISchedulerRule>();
        _doctors = new List<Doctor>();

        Doctors = new ReadOnlyCollection<Doctor>(_doctors);
        SchedulerdDays = new ReadOnlyCollection<ScheduledDay>(_scheduledDays);

        InitializeScheduler();
    }

    public IReadOnlyList<Doctor> Doctors { get; }

    public IReadOnlyList<ScheduledDay> SchedulerdDays { get; }

    public Scheduler AddRule(ISchedulerRule rule)
    {
        ArgumentNullException.ThrowIfNull(rule);

        if (_rules.Contains(rule))
        {
            return this;
        }

        _rules.Add(rule);

        return this;
    }

    public Scheduler AddDoctor(Doctor doctor)
    {
        ArgumentNullException.ThrowIfNull(doctor);

        if (_doctors.Contains(doctor))
        {
            return this;
        }

        _doctors.Add(doctor);

        return this;
    }

    public async Task GenerateAsync(
        IProgress<GenerationProgressInfo> progress,
        CancellationToken cancellationToken = default)
    {
        var generationSource = GenerationSource.Create()
                                               .WithScheduledDays(_scheduledDays)
                                               .WithDoctors(_doctors)
                                               .WithRules(_rules);

        var validationResult = generationSource.Validate();

        if (!validationResult)
        {
            throw new InvalidOperationException(
                message: "Generation source is not valid. Check doctors, workunits and rules collections.");
        }

        var scheduledDays = await _schedulerGenerationStrategy.GenerateAsync(
                                generationSource,
                                progress,
                                cancellationToken);

        for (var dayIndex = 0; dayIndex < scheduledDays.Count; dayIndex++)
        {
            for (var unitIndex = 0; unitIndex < scheduledDays[dayIndex].WorkingUnits.Count; unitIndex++)
            {
                _scheduledDays[dayIndex]
                    .WorkingUnits[unitIndex]
                    .SetDoctor(scheduledDays[dayIndex].WorkingUnits[unitIndex].Doctor);
            }
        }
    }

    public void InitializeScheduler()
    {
        _scheduledDays.Clear();

        foreach (var day in _scheduledDaysProvider.GetScheduledDays())
        {
            _scheduledDays.Add(day);
        }
    }

    public DoctorStatistics GetDoctorStatistics(Doctor doctor)
    {
        ArgumentNullException.ThrowIfNull(doctor);

        var units = SchedulerdDays
                    .SelectMany(day => day.WorkingUnits)
                    .Where(unit => unit.Doctor == doctor)
                    .ToList();

        var days = SchedulerdDays
                   .Where(
                       day => day.WorkingUnits
                                 .Any(unit => unit.Doctor == doctor))
                   .ToList();

        var holidays = SchedulerdDays
                       .Where(day => day.IsHoliday)
                       .Where(day => day.WorkingUnits.Any(unit => unit.Doctor == doctor))
                       .ToList();

        var workingDuration = TimeSpan.FromMinutes(
            SchedulerdDays
                .SelectMany(day => day.WorkingUnits)
                .Where(unit => unit.Doctor == doctor)
                .Sum(d => d.Duration.TotalMinutes));

        var violations = new List<IRuleViolation>();

        foreach (var rule in _rules)
        {
            violations.Add(rule.GetViolationForDoctor(this, doctor));
        }

        var workingDays = SchedulerdDays
            .Count(day => !day.IsHoliday);
        var maxDuration = TimeSpan.FromMinutes(doctor.WorkingDayDuration.TotalMinutes * workingDays);

        return new DoctorStatistics(units, days, holidays, workingDuration, violations, maxDuration);
    }

    public HashSet<int> GetHolidaysIndices()
    {
        var hashSet = new HashSet<int>();

        for (var dayIndex = 0; dayIndex < _scheduledDays.Count; dayIndex++)
        {
            if (_scheduledDays[dayIndex].IsHoliday)
            {
                hashSet.Add(dayIndex);
            }
        }

        return hashSet;
    }

    public IDictionary<int, int[]> GetDoctorsExpectedList()
    {
        var expectedList = new Dictionary<int, int[]>();

        for (var doctorIndex = 0; doctorIndex < _doctors.Count; doctorIndex++)
        {
            var expectedDaysIndices = new List<int>();
            for (var dayIndex = 0; dayIndex < _scheduledDays.Count; dayIndex++)
            {
                if (_doctors[doctorIndex].ExpectedDays.Contains(_scheduledDays[dayIndex].Date))
                {
                    expectedDaysIndices.Add(dayIndex);
                }
            }

            expectedList.Add(doctorIndex, expectedDaysIndices.ToArray());
        }

        return expectedList;
    }
}