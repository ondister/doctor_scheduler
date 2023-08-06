namespace DScheduler.Generation.Genetic;

internal static class Mathematics
{
    public static double CalculateDispersion(double[] items)
    {
        var average = items.Average();
        var sumOfSquaresOfDifferences = items.Select(value => (value - average) * (value - average)).Sum();
        var dispersion = Math.Sqrt(sumOfSquaresOfDifferences / items.Length);

        return dispersion;
    }
}