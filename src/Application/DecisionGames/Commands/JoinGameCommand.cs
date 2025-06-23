using DecisionMate.Application.DecisionGames.Services;
using DecisionMate.Domain.DecisionGames.ValueObjects;
using FluentValidation;
using Geneirodan.Abstractions.Domain;
using Geneirodan.Abstractions.Repositories;
using Geneirodan.MediatR.Abstractions;
using Geneirodan.MediatR.Attributes;
using JetBrains.Annotations;
using MediatR;
using static DecisionMate.Domain.DecisionGames.DecisionGame;
using Result = Ardalis.Result.Result;

namespace DecisionMate.Application.DecisionGames.Commands;

[Authorize]
public sealed record JoinGameCommand(string Code) : ICommand
{
    public sealed class Handler(
        IGameSessionProvider provider,
        IDecisionGameRepository repository,
        IUser user,
        IUnitOfWork unitOfWork,
        IPublisher publisher
    ) : IRequestHandler<JoinGameCommand, Result>
    {
        public async Task<Result> Handle(JoinGameCommand request, CancellationToken cancellationToken)
        {
            var joinCode = new JoinCode(request.Code);
            var gameId = await provider.GetGameSessionByCodeAsync(joinCode).ConfigureAwait(false);
            if (gameId is null)
                return Result.NotFound();

            var game = await repository.FindAsync(gameId.Value, cancellationToken).ConfigureAwait(false);
            if (game is null)
                return Result.NotFound();

            if (game.Players.Any(x => x.Id == user.Id))
                return Result.Conflict();

            var player = new Player { Id = user.Id };
            var @event = game.AddPlayer(player);

            await repository.UpdateAsync(game, cancellationToken).ConfigureAwait(false);
            await unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            await publisher.Publish(@event, cancellationToken).ConfigureAwait(false);


            return Result.Success();
        }
    }

    [UsedImplicitly]
    public sealed class Validator : AbstractValidator<JoinGameCommand>
    {
        public Validator()
        {
            RuleFor(x => x.Code)
                .Length(JoinCode.CodeLength);
        }
    }
}