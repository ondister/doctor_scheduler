namespace DScheduler.Domain.Generation;

public sealed class GenerationSource
{
    public GenerationSource()
    {
        Doctors = Enumerable.Empty<Doctor>();
        WorkingUnits = Enumerable.Empty<WorkingUnit>();
        Rules = Enumerable.Empty<ISchedulerRule>();
    }

    public IEnumerable<Doctor> Doctors { get; private set; }

    public IEnumerable<WorkingUnit> WorkingUnits { get; private set; }

    public IEnumerable<ISchedulerRule> Rules { get; private set; }

    public static GenerationSource Create()
    {
        return new GenerationSource();
    }

    public GenerationSource WithDoctors(IEnumerable<Doctor> doctors)
    {
        ArgumentNullException.ThrowIfNull(doctors);

        Doctors = doctors;

        return this;
    }

    public GenerationSource WithWorkingUnits(IEnumerable<WorkingUnit> workingUnits)
    {
        ArgumentNullException.ThrowIfNull(workingUnits);

        WorkingUnits = workingUnits;

        return this;
    }

    public GenerationSource WithRules(IEnumerable<ISchedulerRule> rules)
    {
        ArgumentNullException.ThrowIfNull(rules);
        Rules = rules;

        return this;
    }

    public bool Validate()
    {
        return Doctors.Any()
            && WorkingUnits.Any()
            && Rules.Any();
    }
}