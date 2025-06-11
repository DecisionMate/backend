namespace DecisionMate.Domain.Common;

public readonly record struct Url(string Value)
{
    public const int MaxLength = 2048;
    
    public override string ToString() => Value;
}