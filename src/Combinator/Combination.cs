using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Combinator
{
    internal sealed class Combination<T> : ICombination<T>
    {
        private readonly Func<T, double> valueSelector;
        private readonly Func<T, double> costSelector;
        private readonly Func<T, int> hashSelector;

        private Combination(
            ImmutableList<T> nodes,
            Func<T, double> valueSelector,
            Func<T, double> costSelector,
            Func<T, int> hashSelector,
            double value,
            double cost,
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

        public double Value { get; }

        public double Cost { get; }

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
            Func<T, double> valueSelector,
            Func<T, double> costSelector,
            Func<T, int> hashSelector) =>
            new Combination<T>(
                nodes: ImmutableList.Create<T>(),
                valueSelector: valueSelector,
                costSelector: costSelector,
                hashSelector: hashSelector,
                value: 0,
                cost: 0,
                hash: 234672346
            );

        public override int GetHashCode() => this.Hash;

        public override bool Equals(object obj)
        {
            return obj is Combination<T> combination &&
                   Nodes.Count == combination.Nodes.Count &&
                   Value == combination.Value &&
                   Cost == combination.Cost &&
                   Hash == combination.Hash;
        }

        public override string ToString()
        {
            return $"value: {Value}, cost: {Cost}, cost/value: {Cost / Value} nodes: {Nodes.Count}";
        }
    }
}