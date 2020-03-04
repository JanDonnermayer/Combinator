using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Combinator
{
    public static class Combinator
    {
        public static IEnumerable<ICombination<T>> Combine<T>(
            IEnumerable<T> nodes,
            Func<T, int> valueSelector,
            int minValue,
            int maxValue,
            Func<T, int> costSelector,
            int maxCost)
        {
            bool proceedPredicate(ICombination<T> state) =>
                state.Value <= maxValue && state.Cost <= maxCost;

            bool choosePredicate(ICombination<T> state) =>
                state.Value >= minValue
                && state.Value <= maxValue
                && state.Cost <= maxCost;

            return Combine(
                nodes: nodes,
                valueSelector: valueSelector,
                costSelector: costSelector,
                proceedPredicate: proceedPredicate,
                choosePredicate: choosePredicate
            );
        }

        public static IEnumerable<ICombination<T>> Combine<T>(
            IEnumerable<T> nodes,
            Func<T, int> valueSelector,
            int minValue,
            int maxValue,
            Func<T, int> costSelector)
        {
            bool proceedPredicate(ICombination<T> state) =>
                state.Value <= maxValue;

            bool choosePredicate(ICombination<T> state) =>
                state.Value >= minValue 
                && state.Value <= maxValue;

            return Combine(
                nodes: nodes,
                valueSelector: valueSelector,
                costSelector: costSelector,
                proceedPredicate: proceedPredicate,
                choosePredicate: choosePredicate
            );
        }

        public static IEnumerable<ICombination<T>> Combine<T>(
            IEnumerable<T> nodes,
            Func<T, int> valueSelector,
            Func<T, int> costSelector,
            int maxCost)
        {
            bool proceedPredicate(ICombination<T> state) =>
                 state.Cost <= maxCost;

            static bool choosePredicate(ICombination<T> _) => true;

            return Combine(
                nodes: nodes,
                valueSelector: valueSelector,
                costSelector: costSelector,
                proceedPredicate: proceedPredicate,
                choosePredicate: choosePredicate
            );
        }

        public static IEnumerable<ICombination<T>> Combine<T>(
            IEnumerable<T> nodes,
            Func<T, int> valueSelector,
            Func<T, int> costSelector,
            Func<ICombination<T>, bool> proceedPredicate,
            Func<ICombination<T>, bool> choosePredicate)
        {
            var rootState = Combination<T>.Empty(
                 valueSelector: valueSelector,
                 costSelector: costSelector,
                 hashSelector: n => n.GetHashCode()
             );

            var comparer = Comparer<Combination<T>>
                .Create((x, y) => x.Cost - y.Cost);

            var frontier = ImmutableSortedSet<Combination<T>>.Empty
                .Add(rootState)
                .WithComparer(comparer);

            var visited = new HashSet<Combination<T>>();

            while (frontier.Count > 0)
            {
                var state = frontier.Min;
                frontier = frontier.Remove(state);

                if (choosePredicate(state)) yield return state;

                frontier = nodes
                    .Select(state.Add)
                    .Where(visited.Add)
                    .Where<Combination<T>>(proceedPredicate)
                    .Aggregate(frontier, (f, n) => f.Add(n));
            }
        }
    }
}