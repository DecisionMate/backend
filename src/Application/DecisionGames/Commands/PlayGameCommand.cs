using Ardalis.Result;
using DecisionMate.Domain.DecisionGames.Abstractions;
using DecisionMate.Domain.DecisionGames.Exceptions;
using DecisionMate.Domain.DecisionGames.ValueObjects;
using Geneirodan.Abstractions.Repositories;
using Geneirodan.MediatR.Abstractions;
using MediatR;
using Result = Ardalis.Result.Result;

namespace DecisionMate.Application.DecisionGames.Commands;

public sealed record PlayGameCommand(Guid GameId) : ICommand<IReadOnlyCollection<DecisionResult>>
{
    public sealed class Handler(
        IDecisionGameRepository repository,
        IAlgorithmResolver resolver,
        IUnitOfWork unitOfWork,
        IPublisher publisher
    ) : IRequestHandler<PlayGameCommand, Result<IReadOnlyCollection<DecisionResult>>>
    {
        public async Task<Result<IReadOnlyCollection<DecisionResult>>> Handle(PlayGameCommand request,
            CancellationToken cancellationToken)
        {
            var game = await repository.FindAsync(request.GameId, cancellationToken).ConfigureAwait(false);
            if (game is null)
                return Result.NotFound();

            if (game.Players.Count < 2)
                return Result.Error(NotEnoughPlayersException.Message);

            var @event = game.Decide(resolver);
            await repository.UpdateAsync(game, cancellationToken).ConfigureAwait(false);
            await unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            await publisher.Publish(@event, cancellationToken).ConfigureAwait(false);

            return Result<IReadOnlyCollection<DecisionResult>>.Success(game.Results);
        }
    }
}