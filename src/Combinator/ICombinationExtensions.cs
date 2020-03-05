namespace Combinator
{
    internal static class ICombinationExtensions
    {
        public static double CostPerValue<T>(this ICombination<T> combination) =>
            (combination.Cost, combination.Value) switch
            {
                (_, 0) => 0,
                (double cost, double value) => cost / value
            };
    }
}