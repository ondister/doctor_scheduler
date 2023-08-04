namespace DScheduler.Domain
{
    public interface IWorkUnitProvider
    {
        IEnumerable<WorkingUnit> GetWorkingUnits();
    }
}
