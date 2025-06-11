using Ardalis.Result;
using DecisionMate.Application.Categories.Contracts;
using DecisionMate.Domain.Users;
using FluentValidation;
using Geneirodan.Abstractions.Domain;
using Geneirodan.Abstractions.Repositories;
using Geneirodan.MediatR.Abstractions;
using Geneirodan.MediatR.Attributes;
using JetBrains.Annotations;
using MediatR;

namespace DecisionMate.Application.UserProfiles.Commands;
[Authorize]
public sealed record AddUserProfileCommand(string Username) : ICommand<Guid>
{
    public sealed class Handler(IUserProfileRepository repository, IUser user, IUnitOfWork unitOfWork)
        : IRequestHandler<AddUserProfileCommand, Result<Guid>>
    {
        public async Task<Result<Guid>> Handle(AddUserProfileCommand request, CancellationToken cancellationToken)
        {
            if (user is not { Id: { } userId })
                return Result.Unauthorized();
            
            var entity = new UserProfile
            {
                Id = userId,
                Username = new UserProfile.Types.Username(request.Username)
            };
            await repository.AddAsync(entity, cancellationToken).ConfigureAwait(false);
            await unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            return Result.Created(userId);
        }
    }
    
    [UsedImplicitly]
    public sealed class Validator : AbstractValidator<AddUserProfileCommand>
    {
        public Validator(IValidator<OptionContract> optionValidator)
        {
            RuleFor(x => x.Username)
                .NotEmpty()
                .Length(UserProfile.Types.Username.MinLength, UserProfile.Types.Username.MaxLength);
        }
    }
}