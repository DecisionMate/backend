namespace DecisionMate.Domain.Common.Collections;

public static class ReadOnlyArrayCollectionInitializer
{
    public static IReadOnlyArray<T> Create<T>(ReadOnlySpan<T> span) => new ReadOnlyArray<T>(span);
}