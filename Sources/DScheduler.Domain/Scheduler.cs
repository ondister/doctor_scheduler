using System.Collections.ObjectModel;

using DScheduler.Domain.Generation;

namespace DScheduler.Domain;

public sealed class Scheduler
{
    private readonly ISchedulerGenerationStrategy _schedulerGenerationStrategy;
    private readonly IList<ISchedulerRule> _rules;
    private readonly IList<Doctor> _doctors;
    private readonly IList<WorkingUnit> _workingUnits;
    private IDictionary<WorkingUnit, Doctor> _schedulerData;

    public Scheduler(ISchedulerGenerationStrategy schedulerGenerationStrategy)
    {
        _schedulerGenerationStrategy = schedulerGenerationStrategy;
        _schedulerData = new Dictionary<WorkingUnit, Doctor>();
        _rules = new List<ISchedulerRule>();
        _doctors = new List<Doctor>();
        _workingUnits = new List<WorkingUnit>();

        SchedulerData = new ReadOnlyDictionary<WorkingUnit, Doctor>(_schedulerData);

        Doctors = new ReadOnlyCollection<Doctor>(_doctors);
    }

    public IReadOnlyCollection<Doctor> Doctors { get; }

    public IReadOnlyDictionary<WorkingUnit, Doctor> SchedulerData { get; }

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

    public void AddWorkingUnit(WorkingUnit workingUnit)
    {
        ArgumentNullException.ThrowIfNull(workingUnit);

        if (_workingUnits.Contains(workingUnit))
        {
            return;
        }

        _workingUnits.Add(workingUnit);
    }

    public async Task GenerateAsync(
        IProgress<GenerationProgressInfo> progress,
        CancellationToken cancellationToken = default)
    {
        var generationSource = GenerationSource.Create()
                                               .WithWorkingUnits(_workingUnits)
                                               .WithDoctors(_doctors)
                                               .WithRules(_rules);

        var validationResult = generationSource.Validate();

        if (!validationResult)
        {
            throw new InvalidOperationException(
                message: "Generation source is not valid. Check doctors, workunits and rules collections.");
        }

        _schedulerData = await _schedulerGenerationStrategy.GenerateAsync(
                             generationSource,
                             progress,
                             cancellationToken);
    }
}