using Ardalis.Result;
using DecisionMate.Domain.Common;
using DecisionMate.Domain.Users;
using DecisionMate.Domain.Users.ValueObjects;
using FluentValidation;
using Geneirodan.Abstractions.Domain;
using Geneirodan.Abstractions.Repositories;
using Geneirodan.MediatR.Abstractions;
using Geneirodan.MediatR.Attributes;
using JetBrains.Annotations;
using MediatR;

namespace DecisionMate.Application.UserProfiles.Commands;

[Authorize]
public sealed record AddUserProfileCommand(string Username, string? AvatarUrl) : ICommand<Guid>
{
    public sealed class Handler(
        IUserProfileRepository repository,
        IUser user,
        IUnitOfWork unitOfWork,
        IPublisher publisher
    ) : IRequestHandler<AddUserProfileCommand, Result<Guid>>
    {
        public async Task<Result<Guid>> Handle(AddUserProfileCommand request, CancellationToken cancellationToken)
        {
            var (entity, @event) = UserProfile.Create(
                id: user.Id,
                userName: new UserName(request.Username),
                avatarUrl: Url.Create(request.AvatarUrl)
            );
            
            await repository.AddAsync(entity, cancellationToken).ConfigureAwait(false);
            await unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            await publisher.Publish(@event, cancellationToken).ConfigureAwait(false);
            
            return Result.Created(user.Id);
        }
    }

    [UsedImplicitly]
    public sealed class Validator : AbstractValidator<AddUserProfileCommand>
    {
        public Validator()
        {
            RuleFor(x => x.Username)
                .NotEmpty()
                .Length(UserName.MinLength, UserName.MaxLength);

            RuleFor(x => x.AvatarUrl)
                .MaximumLength(Url.MaxLength);
        }
    }
}