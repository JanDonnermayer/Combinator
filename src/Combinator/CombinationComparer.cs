using System.Collections.Generic;

namespace Combinator
{
    internal class CombinationComparer<T> : Comparer<ICombination<T>>
    {
        // by(costPerValue).thenByDescending(cost)
        public override int Compare(ICombination<T> x, ICombination<T> y) =>
            (y.CostPerValue() - x.CostPerValue()) switch
            {
                0 => (int)x.Cost - (int)y.Cost,
                double cpv => (int)cpv
            };
    }
}