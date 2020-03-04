using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Combinator
{
    internal sealed class Combination<T> : ICombination<T>
    {
        private readonly Func<T, int> valueSelector;
        private readonly Func<T, int> costSelector;
        private readonly Func<T, int> hashSelector;

        private Combination(
            ImmutableList<T> nodes,
            Func<T, int> valueSelector,
            Func<T, int> costSelector,
            Func<T, int> hashSelector,
            int value,
            int cost,
            int hash)
        {
            Nodes = nodes;
            this.valueSelector = valueSelector;
            this.costSelector = costSelector;
            this.hashSelector = hashSelector;
            Value = value;
            Cost = cost;
            Hash = hash;
        }

        public ImmutableList<T> Nodes { get; }

        public int Value { get; }

        public int Cost { get; }

        public int Hash { get; }

        public Combination<T> Add(T node) =>
            new Combination<T>(
                nodes: this.Nodes.Add(node),
                valueSelector: this.valueSelector,
                costSelector: this.costSelector,
                hashSelector: this.hashSelector,
                value: this.Value + valueSelector(node),
                cost: this.Cost + costSelector(node),
                hash: this.Hash ^ hashSelector(node)
            );

        public static Combination<T> Empty(
            Func<T, int> valueSelector,
            Func<T, int> costSelector,
            Func<T, int> hashSelector) =>
            new Combination<T>(
                nodes: ImmutableList.Create<T>(),
                valueSelector: valueSelector,
                costSelector: costSelector,
                hashSelector: hashSelector,
                value: 0,
                cost: 0,
                hash: 0
            );

        public override bool Equals(object obj)
        {
            return obj is Combination<T> state &&
                   Nodes.SequenceEqual(state.Nodes, EqualityComparer<T>.Default);
        }

        public override int GetHashCode() => this.Hash;
    }
}