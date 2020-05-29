using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Combinator
{
   public static class CombinationProvider
    {
        public static IEnumerable<ICombination<T>> Combine<T>(
            IEnumerable<T> nodes,
            Func<T, double> valueSelector,
            double minValue,
            double maxValue,
            Func<T, double> costSelector,
            double maxCost)
        {
            bool proceedPredicate(ICombination<T> state) =>
                state.Value <= maxValue && state.Cost <= maxCost;

            bool yieldPredicate(ICombination<T> state) =>
                state.Value >= minValue
                && state.Value <= maxValue
                && state.Cost <= maxCost;

            return Combine(
                nodes: nodes,
                valueSelector: valueSelector,
                costSelector: costSelector,
                proceedPredicate: proceedPredicate,
                yieldPredicate: yieldPredicate
            );
        }

        public static IEnumerable<ICombination<T>> Combine<T>(
            IEnumerable<T> nodes,
            Func<T, double> valueSelector,
            double minValue,
            double maxValue,
            Func<T, double> costSelector)
        {
            bool proceedPredicate(ICombination<T> state) =>
                state.Value <= maxValue;

            bool yieldPredicate(ICombination<T> state) =>
                state.Value >= minValue
                && state.Value <= maxValue;

            return Combine(
                nodes: nodes,
                valueSelector: valueSelector,
                costSelector: costSelector,
                proceedPredicate: proceedPredicate,
                yieldPredicate: yieldPredicate
            );
        }

        public static IEnumerable<ICombination<T>> Combine<T>(
            IEnumerable<T> nodes,
            Func<T, double> valueSelector,
            Func<T, double> costSelector,
            double minCost,
            double maxCost)
        {
            bool proceedPredicate(ICombination<T> state) =>
                state.Cost <= maxCost;

            bool yieldPredicate(ICombination<T> state) =>
                state.Cost >= minCost;

            return Combine(
                nodes: nodes,
                valueSelector: valueSelector,
                costSelector: costSelector,
                proceedPredicate: proceedPredicate,
                yieldPredicate: yieldPredicate
            );
        }

        public static IEnumerable<ICombination<T>> Combine<T>(
            IEnumerable<T> nodes,
            Func<T, double> valueSelector,
            Func<T, double> costSelector,
            Func<ICombination<T>, bool> proceedPredicate,
            Func<ICombination<T>, bool> yieldPredicate)
        {
            var rootState = Combination<T>.Empty(
                valueSelector: valueSelector,
                costSelector: costSelector,
                hashSelector: n => n.GetHashCode()
             );

            var frontier = ImmutableHashSet<Combination<T>>.Empty
                .Add(rootState);

            var visited = new HashSet<Combination<T>>();

            while (frontier.Count > 0)
            {
                var state = frontier
                    .OrderBy(s => s.CostPerValue())
                    .ThenByDescending(s => s.Value)
                    .First();

                frontier = frontier.Remove(state);

                if (yieldPredicate(state))
                    yield return state;

                frontier = nodes
                    .Select(state.Add)
                    .Where(visited.Add)
                    .Where<Combination<T>>(proceedPredicate)
                    .Aggregate(frontier, (f, n) => f.Add(n));
            }
        }
    }
}