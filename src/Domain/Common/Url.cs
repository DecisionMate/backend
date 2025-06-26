namespace DecisionMate.Domain.Common;

public readonly record struct Url(string Value)
{
    public static Url? Create(string? value) => value is null ? null : new Url(value);

    public const int MaxLength = 2048;

    public override string ToString() => Value;
}