using Ardalis.Result;
using DecisionMate.Domain.Users;
using Geneirodan.Abstractions.Repositories;
using Geneirodan.MediatR.Abstractions;
using MediatR;

namespace DecisionMate.Application.UserProfiles.Commands;

public sealed record DeleteUserProfileCommand(Guid Id) : ICommand
{
    public sealed class Handler(IRepository<UserProfile, Guid> repository, IUnitOfWork unitOfWork, IPublisher publisher)
        : IRequestHandler<DeleteUserProfileCommand, Result>
    {
        public async Task<Result> Handle(DeleteUserProfileCommand request, CancellationToken cancellationToken)
        {
            var entity = await repository.FindAsync(request.Id, cancellationToken).ConfigureAwait(false);
            if (entity is null)
                return Result.NotFound();

            var @event = entity.Delete();

            await repository.DeleteAsync(entity, cancellationToken).ConfigureAwait(false);
            await unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            await publisher.Publish(@event, cancellationToken).ConfigureAwait(false);
            
            return Result.Success();
        }
    }
}