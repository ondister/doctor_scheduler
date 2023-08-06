using DScheduler.Domain;
using DScheduler.Domain.Generation;

using GeneticSharp;

namespace DScheduler.Generation.Genetic;

public sealed class GeneticSchedulerGenerationStrategy : ISchedulerGenerationStrategy
{
    Task<IList<ScheduledDay>> ISchedulerGenerationStrategy.GenerateAsync(
        GenerationSource source,
        IProgress<GenerationProgressInfo> progress,
        CancellationToken cancellationToken)
    {
        var taskCompletionSource = new TaskCompletionSource<IList<ScheduledDay>>();

        try
        {
            var random = new Random();

            var doctors = source.Doctors.ToList();
            var schedulerDays = source.ScheduledDays.ToList();

            var chromosome = new ScheduleChromosome(schedulerDays.Count, doctors.Count);
            var population = new Population(minSize: 1000, maxSize: 10000, chromosome);

            var fitness = new ScheduleFithness(source.Rules);
            IMutation mutation = new ReverseSequenceMutation();
            ISelection selection = new EliteSelection();
            var point1 = random.Next(schedulerDays.Count - 2);
            ICrossover crossover = new TwoPointCrossover(point1, random.Next(point1 + 1, schedulerDays.Count - 4));

            var geneticAlgorithm = new GeneticAlgorithm(population, fitness, selection, crossover, mutation);
            geneticAlgorithm.MutationProbability = 0.1f;

            ITermination termination = new FitnessStagnationTermination(expectedStagnantGenerationsNumber: 30);
            geneticAlgorithm.Termination = termination;

            // Task cancelation.
            cancellationToken.Register(
                () =>
                {
                    geneticAlgorithm.Stop();
                    taskCompletionSource.SetCanceled();
                });

            geneticAlgorithm.GenerationRan += (sender, args) =>
            {
                // Task progress.
                if (sender is GeneticAlgorithm geneticAlgorithmSender)
                {
                    progress.Report(
                        new GenerationProgressInfo(
                            geneticAlgorithmSender.GenerationsNumber,
                            geneticAlgorithmSender.TimeEvolving,
                            GenerationStatus.InProgress));
                }
            };

            geneticAlgorithm.Stopped += (sender, args) =>
            {
                if (sender is GeneticAlgorithm geneticAlgorithmSender)
                {
                    progress.Report(
                        new GenerationProgressInfo(
                            geneticAlgorithmSender.GenerationsNumber,
                            geneticAlgorithmSender.TimeEvolving,
                            GenerationStatus.Cancelled));
                }
            };

            geneticAlgorithm.TerminationReached += (sender, args) =>
            {
                if (sender is GeneticAlgorithm geneticAlgorithmSender)
                {
                    progress.Report(
                        new GenerationProgressInfo(
                            geneticAlgorithm.GenerationsNumber,
                            geneticAlgorithm.TimeEvolving,
                            GenerationStatus.Finished));
                }

                // Create task result.
                var bestGenes = geneticAlgorithm.BestChromosome.GetGenes();
                var result = new List<ScheduledDay>();

                for (var dayIndex = 0; dayIndex < schedulerDays.Count; dayIndex++)
                {
                    var scheduledDay = schedulerDays[dayIndex];
                    var doctorIndex = ((int[])bestGenes[dayIndex].Value)[0];
                    var doctor = doctors[doctorIndex];
                    scheduledDay.WorkingUnits[index: 0].SetDoctor(doctor);

                    result.Add(scheduledDay);
                }

                taskCompletionSource.SetResult(result);
            };

            // TODO: Handle exceptions
            Task.Run(() => { geneticAlgorithm.Start(); });
        }
        catch (Exception exception)
        {
            taskCompletionSource.SetException(exception);
        }

        return taskCompletionSource.Task;
    }
}