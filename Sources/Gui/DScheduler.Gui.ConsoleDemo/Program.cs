using DScheduler.Domain;
using DScheduler.Domain.Generation;
using DScheduler.Generation.Genetic;

using DSchedulerGeneration.Genetic.Rules;

var calendar = new CalendarDayWorkingUnitProvider(new DateOnly(2023, 8, 1), new DateOnly(2023, 8, 31));
var workingUnits = calendar.GetWorkingUnits();

var scheduler = new Scheduler(new GeneticSchedulerGenerationStrategy());

foreach (var workingUnit in workingUnits)
{
    scheduler.AddWorkingUnit(workingUnit);
}

scheduler.AddDoctor(new Doctor("Doctor 1"))
         .AddDoctor(new Doctor("Doctor 2"))
         .AddDoctor(new Doctor("Doctor 3"))
         .AddDoctor(new Doctor("Doctor 4"))
         .AddDoctor(new Doctor("Doctor 5"));


scheduler.AddRule(new DaysDispersionSchedulerRule(0.5,scheduler.Doctors.Count));

scheduler.GenerateAsync(new Progress<GenerationProgressInfo>());

Console.ReadKey();