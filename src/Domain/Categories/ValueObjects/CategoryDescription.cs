namespace DecisionMate.Domain.Categories.ValueObjects;

public readonly record struct CategoryDescription(string Value)
{
    public static int MaxLength => 1024;
}