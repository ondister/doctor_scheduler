using DScheduler.Domain;
using DScheduler.Domain.Generation;
using DScheduler.Generation.Genetic;

using DSchedulerGeneration.Genetic.Rules;

var calendar = new SingleWorkUnitScheduledDaysProvider(
    new DateOnly(year: 2023, month: 8, day: 1),
    new DateOnly(year: 2023, month: 8, day: 31));

var scheduler = new Scheduler(calendar, new GeneticSchedulerGenerationStrategy());

scheduler.AddDoctor(
             new Doctor(name: "Doctor 1")
                 .AddExpectedDay(new DateOnly(year: 2023, month: 8, day: 3))
                 .AddExpectedDay(new DateOnly(year: 2023, month: 8, day: 4)))
.AddDoctor(new Doctor(name: "Doctor 2").SetWorkingDayDuration(new TimeSpan(7,42,0)))
.AddDoctor(new Doctor(name: "Doctor 3").SetWorkingDayDuration(new TimeSpan(7, 42, 0)))
.AddDoctor(new Doctor(name: "Doctor 4"));

scheduler.AddRule(new HolidaysDispersionSchedulerRule(weight: 1, scheduler.Doctors.Count, scheduler.GetHolidaysIndices()))
         .AddRule(new DaysDispersionSchedulerRule(weight: 1, scheduler.Doctors.Count))
         .AddRule(new RestSecondDaySchedulerRule(weight: 2))
         .AddRule(new RestFirstDaySchedulerRule(weight: 3))
         .AddRule(new DaysExceptionSchedulerRule(4, scheduler.GetDoctorsExpectedList()))
         .AddRule(new MaxDaysCountSchedulerRule(4, scheduler));

var progress = new Progress<GenerationProgressInfo>();
progress.ProgressChanged += OnProgressChanged;

void OnProgressChanged(object? sender, GenerationProgressInfo e)
{
    Console.WriteLine($"{e.GenerationSttus} - {e.GenerationsNumber} - {e.TimeEvolving}");
}

await scheduler.GenerateAsync(progress);

foreach (var day in scheduler.SchedulerdDays)
{
    var dayStatistics = day.GetStatistics();

    foreach (var unit in day.WorkingUnits)
    {
        Console.WriteLine($"{unit.ScheduledDay} - {unit.Doctor}");
    }
}

foreach (var doctor in scheduler.Doctors)
{
    Console.WriteLine($"{doctor}-------------");
    var doctorStatistics = scheduler.GetDoctorStatistics(doctor);

    Console.WriteLine($"Days count {doctorStatistics.Days.Count}");
    Console.WriteLine($"Holidays count {doctorStatistics.Holidays.Count}");
    Console.WriteLine($"Units cout {doctorStatistics.Units.Count}");
    Console.WriteLine($"Duration {doctorStatistics.WorkingDuration.ToString()}");
    Console.WriteLine($"MaxDuration {doctorStatistics.MaxDuration.ToString()}");

    foreach (var violation in doctorStatistics.Violations)
    {
        Console.WriteLine(violation);
    }

    Console.WriteLine(value: "-------------");
}

Console.ReadKey();