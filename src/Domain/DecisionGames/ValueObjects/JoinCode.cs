namespace DecisionMate.Domain.DecisionGames.ValueObjects;

public readonly record struct JoinCode(string Value)
{
    public const int CodeLength = 4;
    public override string ToString() => Value;
}