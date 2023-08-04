namespace DScheduler.Domain
{
    public abstract class WorkingUnit
    {
        public bool IsHoliday { get; set; }

        public string Name { get; }

        protected WorkingUnit(string name)
        {
            Name = name;
        }
    }
}