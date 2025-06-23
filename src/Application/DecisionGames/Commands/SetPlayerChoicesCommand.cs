using System.Globalization;
using Ardalis.Result;
using DecisionMate.Application.Common.Resources;
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
public sealed record SetPlayerChoicesCommand(Guid GameId, int[] Choices) : ICommand
{
    public sealed class Handler(
        IDecisionGameRepository repository,
        IUser user,
        IUnitOfWork unitOfWork,
        IPublisher publisher
    ) : IRequestHandler<SetPlayerChoicesCommand, Result>
    {
        public async Task<Result> Handle(SetPlayerChoicesCommand request, CancellationToken cancellationToken)
        {
            var (gameId, choices) = request;

            var game = await repository.FindAsync(gameId, cancellationToken).ConfigureAwait(false);
            if (game is null)
                return Result.NotFound();

            if (game.Players.All(x => x.Id != user.Id))
                return Result.Conflict();

            if (game.Settings.TopSize != choices.Length)
            {
                var errorMessage = string.Format(
                    provider: CultureInfo.CurrentUICulture,
                    format: ErrorMessages.InvalidTopSize,
                    arg0: game.Settings.TopSize
                );
                var validationError = new ValidationError(nameof(Choices), errorMessage);
                return Result.Invalid(validationError);
            }

            var @event = game.SetPlayerChoices(user.Id, [..choices]);
            
            await repository.UpdateAsync(game, cancellationToken).ConfigureAwait(false);
            await unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            await publisher.Publish(@event, cancellationToken).ConfigureAwait(false);
            
            return Result.Success();
        }
    }

    [UsedImplicitly]
    public sealed class Validator : AbstractValidator<SetPlayerChoicesCommand>
    {
        public Validator() => RuleFor(x => x.Choices).NotEmpty();
    }
}