using DecisionMate.Domain.Categories.Options.ValueObjects;
using FluentValidation;
using JetBrains.Annotations;

namespace DecisionMate.Application.Categories.Contracts;

[PublicAPI]
public sealed record OptionContract(string Name, string Description, string? ImageUrl)
{
    [UsedImplicitly]
    public sealed class Validator : AbstractValidator<OptionContract>
    {
        public Validator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .Length(OptionName.MinLength, OptionName.MaxLength);
            
            RuleFor(x => x.Description)
                .NotEmpty()
                .MaximumLength(OptionDescription.MaxLength);
        }
    }
}