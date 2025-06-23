using Ardalis.Result;
using DecisionMate.Domain.DecisionGames;
using Geneirodan.Abstractions.Domain;
using Geneirodan.Abstractions.Repositories;
using Geneirodan.MediatR.Abstractions;
using Geneirodan.MediatR.Attributes;
using MediatR;

namespace DecisionMate.Application.DecisionGames.Commands;

[Authorize]
public sealed record DeleteGameCommand(Guid Id) : ICommand
{
    public sealed class Handler(
        IRepository<DecisionGame, Guid> repository,
        IUnitOfWork unitOfWork,
        IPublisher publisher,
        IUser user
    ) : IRequestHandler<DeleteGameCommand, Result>
    {
        public async Task<Result> Handle(DeleteGameCommand request, CancellationToken cancellationToken)
        {
            var game = await repository.FindAsync(request.Id, cancellationToken).ConfigureAwait(false);
            if (game is null)
                return Result.NotFound();
            
            if(user.Id != game.HostId)
                return Result.Forbidden();
            
            var @event = game.Delete();
            
            await repository.DeleteAsync(game, cancellationToken).ConfigureAwait(false);
            await unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            await publisher.Publish(@event, cancellationToken).ConfigureAwait(false);
            
            return Result.Success();
        }
    }
}