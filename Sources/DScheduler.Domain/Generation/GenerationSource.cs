using DScheduler.Domain.Rules;

namespace DScheduler.Domain.Generation;

public sealed class GenerationSource
{
    public GenerationSource()
    {
        Doctors = Enumerable.Empty<Doctor>();
        ScheduledDays = Enumerable.Empty<ScheduledDay>();
        Rules = Enumerable.Empty<ISchedulerRule>();
    }

    public IEnumerable<Doctor> Doctors { get; private set; }

    public IEnumerable<ISchedulerRule> Rules { get; private set; }

    public IEnumerable<ScheduledDay> ScheduledDays { get; private set; }

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

    public GenerationSource WithScheduledDays(IEnumerable<ScheduledDay> scheduledDays)
    {
        ArgumentNullException.ThrowIfNull(scheduledDays);

        ScheduledDays = scheduledDays;

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
            && ScheduledDays.Any()
            && Rules.Any()
            && ScheduledDays.All(day => day.WorkingUnits.Any());
    }
}