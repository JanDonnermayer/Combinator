using System.Collections.Immutable;

namespace Combinator
{
    public interface ICombination<T>
    {
        ImmutableList<T> Nodes { get; }
        double Value { get; }
        double Cost { get; }
    }
}