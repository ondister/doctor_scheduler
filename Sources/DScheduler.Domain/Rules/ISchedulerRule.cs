namespace DScheduler.Domain.Rules
{
    public interface ISchedulerRule
    {
        double Weight { get; }

        double Calculate();

        IRuleViolation GetViolationForDoctor(Scheduler scheduler, Doctor doctor);
    }
}