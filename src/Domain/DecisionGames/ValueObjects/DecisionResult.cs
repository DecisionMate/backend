namespace DecisionMate.Domain.DecisionGames.ValueObjects;

public sealed record DecisionResult(int Option, int Points)
{
    public static implicit operator DecisionResult((int, int) tuple) => new(tuple.Item1, tuple.Item2);
}