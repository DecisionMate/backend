using Ardalis.Result;
using Geneirodan.Abstractions.Domain;
using Geneirodan.Abstractions.Repositories;
using Geneirodan.MediatR.Abstractions;
using Geneirodan.MediatR.Attributes;
using MediatR;

namespace DecisionMate.Application.DecisionGames.Commands;

[Authorize]
public sealed record LeaveGameCommand(Guid GameId) : ICommand
{
    public sealed class Handler(
        IDecisionGameRepository repository,
        IUser user,
        IUnitOfWork unitOfWork,
        IPublisher publisher
    ) : IRequestHandler<LeaveGameCommand, Result>
    {
        public async Task<Result> Handle(LeaveGameCommand request, CancellationToken cancellationToken)
        {
            var game = await repository.FindAsync(request.GameId, cancellationToken).ConfigureAwait(false);
            if (game is null)
                return Result.NotFound();

            if (game.Players.All(x => x.Id != user.Id))
                return Result.Forbidden();

            var @event = game.RemovePlayer(user.Id);

            await repository.UpdateAsync(game, cancellationToken).ConfigureAwait(false);
            await unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            await publisher.Publish(@event, cancellationToken).ConfigureAwait(false);
            
            return Result.Success();
        }
    }
}