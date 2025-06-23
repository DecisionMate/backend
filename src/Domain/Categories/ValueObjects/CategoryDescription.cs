namespace DecisionMate.Domain.Categories.ValueObjects;

public readonly record struct CategoryDescription(string Value)
{
    public static int MaxLength => 1024;
    public override string ToString() => Value;

    public static CategoryDescription? Create(string? value) => value is null ? null : new CategoryDescription(value);
}