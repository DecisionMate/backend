namespace DecisionMate.Domain.Users.ValueObjects;

public record struct UserName(string Value)
{
    public const int MinLength = 2;
    public const int MaxLength = 32;
}