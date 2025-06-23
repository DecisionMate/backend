using Ardalis.Result;
using DecisionMate.Application.DecisionGames.Services;
using Geneirodan.Abstractions.Domain;
using Geneirodan.Abstractions.Repositories;
using Geneirodan.MediatR.Abstractions;
using Geneirodan.MediatR.Attributes;
using MediatR;

namespace DecisionMate.Application.DecisionGames.Commands;

[Authorize]
public sealed record CreateNewCodeCommand(Guid GameId) : ICommand<string>
{
    public sealed class Handler(
        IDecisionGameRepository repository,
        IGameSessionProvider gameSessionProvider,
        IUnitOfWork unitOfWork,
        IPublisher publisher,
        IUser user
    ) : IRequestHandler<CreateNewCodeCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(CreateNewCodeCommand request, CancellationToken cancellationToken)
        {
            var game = await repository.FindAsync(request.GameId, cancellationToken).ConfigureAwait(false);
            if (game is null)
                return Result<string>.NotFound();

            if (user.Id != game.HostId)
                return Result<string>.Forbidden();

            if (game.Code is not null
                && game.Id == await gameSessionProvider.GetGameSessionByCodeAsync(game.Code.Value).ConfigureAwait(false)
               )
                await gameSessionProvider.RemoveCodeAsync(game.Code.Value).ConfigureAwait(false);

            var code = await gameSessionProvider.CreateCodeAsync(game.Id).ConfigureAwait(false);

            var @event = game.UpdateCode(code);

            await repository.UpdateAsync(game, cancellationToken).ConfigureAwait(false);
            await unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            await publisher.Publish(@event, cancellationToken).ConfigureAwait(false);

            return Result.Created(game.Code!.Value.Value);
        }
    }
}