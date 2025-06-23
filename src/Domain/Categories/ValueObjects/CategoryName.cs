namespace DecisionMate.Domain.Categories.ValueObjects;

public readonly record struct CategoryName(string Value)
{
    public static int MinLength => 2;
    public static int MaxLength => 50;
    public override string ToString() => Value;
}