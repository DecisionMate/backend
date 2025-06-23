namespace DecisionMate.Domain.Categories.Options.ValueObjects;

public readonly record struct OptionName(string Value)
{
    public static int MinLength => 1;
    public static int MaxLength => 50;
    public override string ToString() => Value;
}