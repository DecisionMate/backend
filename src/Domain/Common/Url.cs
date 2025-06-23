namespace DecisionMate.Domain.Common;

public readonly record struct Url
{
    private Url(string Value) => this.Value = Value;

    public static Url? Create(string? value) => value is null ? null : new Url(value);

    public const int MaxLength = 2048;
    public string Value { get; }

    public override string ToString() => Value;
}