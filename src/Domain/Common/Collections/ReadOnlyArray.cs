using System.Collections;

namespace DecisionMate.Domain.Common.Collections;

public class ReadOnlyArray<T>(ReadOnlySpan<T> choices) : IReadOnlyArray<T>
{
    private readonly T[] _choices = choices.ToArray();

    public int Size => _choices.Length;

    public T this[int index] => _choices[index];

    public IEnumerator<T> GetEnumerator() => _choices.AsEnumerable().GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => _choices.GetEnumerator();

}