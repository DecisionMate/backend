using Ardalis.Result;
using DecisionMate.Application.Categories.Contracts;
using DecisionMate.Application.Categories.Mappers;
using DecisionMate.Domain.Categories;
using DecisionMate.Domain.Categories.ValueObjects;
using DecisionMate.Domain.Common;
using FluentValidation;
using Geneirodan.Abstractions.Repositories;
using Geneirodan.MediatR.Abstractions;
using JetBrains.Annotations;
using MediatR;

namespace DecisionMate.Application.Categories.Commands;

public sealed record EditCategoryCommand(
    Guid Id,
    string Name,
    string Description,
    OptionContract[] Options,
    string? ImageUrl
) : ICommand<CategoryModel>
{
    public sealed class Handler(IRepository<Category, Guid> repository, IUnitOfWork unitOfWork, IPublisher publisher)
        : IRequestHandler<EditCategoryCommand, Result<CategoryModel>>
    {
        public async Task<Result<CategoryModel>> Handle(EditCategoryCommand request,
            CancellationToken cancellationToken)
        {
            var entity = await repository.FindAsync(request.Id, cancellationToken).ConfigureAwait(false);
            if (entity is null)
                return Result<CategoryModel>.NotFound();
            
            var @event = entity.Edit(
                name: new CategoryName(request.Name),
                description: new CategoryDescription(request.Description),
                options: request.Options.Map(),
                imageUrl: Url.Create(request.ImageUrl)
            );
            
            await repository.UpdateAsync(entity, cancellationToken).ConfigureAwait(false);
            await unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            await publisher.Publish(@event, cancellationToken).ConfigureAwait(false);
            
            return entity.MapToModel();
        }
    }

    [UsedImplicitly]
    public sealed class Validator : AbstractValidator<EditCategoryCommand>
    {
        public Validator(IValidator<OptionContract> optionValidator)
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .Length(CategoryName.MinLength, CategoryName.MaxLength);

            RuleFor(x => x.Description)
                .NotEmpty()
                .MaximumLength(CategoryDescription.MaxLength);

            RuleFor(x => x.Options)
                .ForEach(x => x.SetValidator(optionValidator));
        }
    }
}