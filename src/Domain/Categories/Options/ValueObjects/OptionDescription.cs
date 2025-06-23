namespace DecisionMate.Domain.Categories.Options.ValueObjects;

public readonly record struct OptionDescription(string Value)
{
    public static int MaxLength => 1024;
    public override string ToString() => Value;
}