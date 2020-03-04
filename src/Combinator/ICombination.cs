using System.Collections.Immutable;

namespace Combinator
{
    public interface ICombination<T>
    {
        ImmutableList<T> Nodes { get; }
        int Value { get; }
        int Cost { get; }
    }
}