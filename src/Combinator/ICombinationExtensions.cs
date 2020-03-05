namespace Combinator
{
    internal static class ICombinationExtensions
    {
        public static int CostPerValue<T>(this ICombination<T> combination) =>
            combination.Cost / System.Math.Max(combination.Value, 1);
    }
}