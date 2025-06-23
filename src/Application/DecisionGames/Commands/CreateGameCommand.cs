using Ardalis.Result;
using DecisionMate.Application.Common.Resources;
using DecisionMate.Application.DecisionGames.Mappers;
using DecisionMate.Application.DecisionGames.Validators;
using DecisionMate.Domain.Categories;
using DecisionMate.Domain.DecisionGames;
using DecisionMate.Domain.DecisionGames.Models;
using DecisionMate.Domain.DecisionGames.ValueObjects;
using FluentValidation;
using Geneirodan.Abstractions.Domain;
using Geneirodan.Abstractions.Repositories;
using Geneirodan.MediatR.Abstractions;
using Geneirodan.MediatR.Attributes;
using JetBrains.Annotations;
using MediatR;

namespace DecisionMate.Application.DecisionGames.Commands;

[Authorize]
public sealed record CreateGameCommand(Guid CategoryId, GameSettings Settings)
    : ICommand<DecisionGameCreatedModel>
{
    public sealed class Handler(
        IRepository<DecisionGame, Guid> repository,
        IUnitOfWork unitOfWork,
        IPublisher publisher,
        IUser user
    ) : IRequestHandler<CreateGameCommand, Result<DecisionGameCreatedModel>>
    {
        public async Task<Result<DecisionGameCreatedModel>> Handle(CreateGameCommand request, CancellationToken token)
        {
            var (categoryId, gameSettings) = request;

            var (game, @event) = DecisionGame.Create(categoryId, gameSettings, user.Id);
            
            game = await repository.AddAsync(game, token).ConfigureAwait(false);
            await unitOfWork.SaveChangesAsync(token).ConfigureAwait(false);
            await publisher.Publish(@event, token).ConfigureAwait(false);
            
            var model = game.MapToCreatedModel();

            return Result<DecisionGameCreatedModel>.Created(model);
        }
    }

    [UsedImplicitly]
    public sealed class Validator : AbstractValidator<CreateGameCommand>
    {
        public Validator(IRepository<Category, Guid> categoryRepository, GameSettingsValidator gameSettingsValidator)
        {
            RuleFor(x => x.CategoryId)
                .NotEmpty()
                .MustAsync(async (x, ct) => await categoryRepository.ExistsAsync(x, ct).ConfigureAwait(false))
                .WithMessage(ErrorMessages.CategoryNotFound);

            RuleFor(x => x.Settings)
                .NotEmpty()
                .SetValidator(gameSettingsValidator);
        }
    }
}