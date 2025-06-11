using DecisionMate.Domain.DecisionGames.ValueObjects;
using FluentValidation;
using JetBrains.Annotations;

namespace DecisionMate.Application.DecisionGames.Validators;

[UsedImplicitly]
public sealed class GameSettingsValidator : AbstractValidator<GameSettings>
{
    public GameSettingsValidator()
    {
        RuleFor(x => x.Type)
            .NotEmpty()
            .IsInEnum();

        RuleFor(x => x.Algorithm)
            .NotEmpty()
            .IsInEnum();

        RuleFor(x => x.NumberOfWinners)
            .GreaterThanOrEqualTo(GameSettings.MinNumberOfWinners)
            .LessThanOrEqualTo(GameSettings.MaxNumberOfWinners);

        RuleFor(x => x.TopSize)
            .GreaterThan(GameSettings.MinTopSize)
            .LessThan(GameSettings.MaxTopSize);
    }
}