using System.Runtime.CompilerServices;

namespace DecisionMate.Domain.Common.Collections;

[CollectionBuilder(typeof(ReadOnlyArrayCollectionInitializer), nameof(ReadOnlyArrayCollectionInitializer.Create))]
public interface IReadOnlyArray<out T> : IEnumerable<T>
{
    int Size { get; }
    T this[int index] { get; }
}

