namespace DScheduler.Domain
{
    public interface ISchedulerRule
    {
        double Weight { get; }

        double Calculate();
    }
}