using Ardalis.Result;
using DecisionMate.Domain.DecisionGames;
using Geneirodan.Abstractions.Repositories;
using Geneirodan.MediatR.Abstractions;
using MediatR;

namespace DecisionMate.Application.DecisionGames.Commands;

public sealed record DeleteGameCommand(Guid Id) : ICommand
{
    public sealed class Handler(
        IRepository<DecisionGame, Guid> repository,
        IUnitOfWork unitOfWork,
        IPublisher publisher
    ) : IRequestHandler<DeleteGameCommand, Result>
    {
        public async Task<Result> Handle(DeleteGameCommand request,
            CancellationToken cancellationToken)
        {
            var game = await repository.FindAsync(request.Id, cancellationToken).ConfigureAwait(false);
            if (game is null)
                return Result.NotFound();
            var @event = game.Delete();
            await repository.DeleteAsync(game, cancellationToken).ConfigureAwait(false);
            await unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            await publisher.Publish(@event, cancellationToken).ConfigureAwait(false);
            return Result.Success();
        }
    }
}