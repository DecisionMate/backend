using Ardalis.Result;
using DecisionMate.Application.DecisionGames.Mappers;
using DecisionMate.Application.DecisionGames.Validators;
using DecisionMate.Domain.Categories;
using DecisionMate.Domain.Common;
using DecisionMate.Domain.DecisionGames;
using DecisionMate.Domain.DecisionGames.Models;
using DecisionMate.Domain.DecisionGames.ValueObjects;
using FluentValidation;
using Geneirodan.Abstractions.Domain;
using Geneirodan.Abstractions.Repositories;
using Geneirodan.MediatR.Abstractions;
using Geneirodan.MediatR.Attributes;
using JetBrains.Annotations;
using MediatR;
using Result = Ardalis.Result.Result;

namespace DecisionMate.Application.DecisionGames.Commands;

[Authorize]
public sealed record UpdateGameCommand(Guid Id, GameSettings Settings, string? ImageUrl) : ICommand<DecisionGameModel>
{
    public sealed class Handler(
        IRepository<DecisionGame, Guid> repository,
        IUnitOfWork unitOfWork,
        IPublisher publisher,
        IUser user
    ) : IRequestHandler<UpdateGameCommand, Result<DecisionGameModel>>
    {
        public async Task<Result<DecisionGameModel>> Handle(UpdateGameCommand request,
            CancellationToken cancellationToken)
        {
            var game = await repository.FindAsync(request.Id, cancellationToken).ConfigureAwait(false);
            if (game is null)
                return Result.NotFound();

            if (user.Id != game.HostId)
                return Result.Forbidden();

            var @event = game.Update(request.Settings);
            
            game = await repository.UpdateAsync(game, cancellationToken).ConfigureAwait(false);
            await unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            await publisher.Publish(@event, cancellationToken).ConfigureAwait(false);
            
            return game.MapToModel();
        }
    }

    [UsedImplicitly]
    public sealed class Validator : AbstractValidator<UpdateGameCommand>
    {
        public Validator(IRepository<Category, Guid> categoryRepository, GameSettingsValidator gameSettingsValidator)
        {
            RuleFor(x => x.Settings)
                .NotEmpty()
                .SetValidator(gameSettingsValidator);

            RuleFor(x => x.ImageUrl)
                .MaximumLength(Url.MaxLength)
                .When(x => !string.IsNullOrEmpty(x.ImageUrl));
        }
    }
}