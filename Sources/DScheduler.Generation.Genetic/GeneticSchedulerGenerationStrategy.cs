using DScheduler.Domain;
using DScheduler.Domain.Generation;

using GeneticSharp;

namespace DScheduler.Generation.Genetic;

public sealed class GeneticSchedulerGenerationStrategy : ISchedulerGenerationStrategy
{
    /// <inheritdoc />
    public async Task<IDictionary<WorkingUnit, Doctor>> GenerateAsync(
        GenerationSource source,
        IProgress<GenerationProgressInfo> progress,
        CancellationToken cancellationToken)
    {
      
        var random= new Random();
        var workingUnitsCount = source.WorkingUnits.Count();
        var chromosome = new ScheduleChromosome(workingUnitsCount, source.Doctors.Count());
        var population = new Population(1000, 10000, chromosome);

        var fitness = new ScheduleFithness(source.Rules);
        IMutation mutation = new ReverseSequenceMutation();
        ISelection selection = new EliteSelection();
        var p1 = random.Next(workingUnitsCount-1);
        ICrossover crossover = new TwoPointCrossover(p1, random.Next(p1 + 1, workingUnitsCount-1));


        var ga = new GeneticAlgorithm(population, fitness, selection, crossover, mutation);

        ITermination termination = new FitnessStagnationTermination(100);

        ga.Termination = termination;

        ga.MutationProbability = 0.1f;

        ga.Start();

        Gene[] g = ga.BestChromosome.GetGenes();

        Console.WriteLine("--------------");
        foreach (Gene gene in g)
        {
            Console.WriteLine(((int[])gene.Value)[0]);
        }

        Console.WriteLine("----");
        for (var doctorIndex = 0; doctorIndex < source.Doctors.Count(); doctorIndex++)
        {
            double cc = g.Count(v => ((int[])v.Value)[0] == doctorIndex);

            Console.WriteLine($"{doctorIndex}_{cc}");
        }

        return await Task.FromResult(new Dictionary<WorkingUnit, Doctor>());
    }
}