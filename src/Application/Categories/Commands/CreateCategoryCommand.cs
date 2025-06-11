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

public sealed record CreateCategoryCommand(
    string Name,
    string Description,
    OptionContract[] Options,
    string? ImageUrl
) : ICommand<CategoryModel>
{
    public sealed class Handler(IRepository<Category, Guid> repository, IUnitOfWork unitOfWork)
        : IRequestHandler<CreateCategoryCommand, Result<CategoryModel>>
    {
        public async Task<Result<CategoryModel>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            var entity = request.MapToCategory();
            entity = await repository.AddAsync(entity, cancellationToken).ConfigureAwait(false);
            await unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            var response = entity.MapToModel();
            return Result<CategoryModel>.Created(response);
        }
    }
    
    [UsedImplicitly]
    public sealed class Validator : AbstractValidator<CreateCategoryCommand>
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
            
            RuleFor(x => x.ImageUrl)
                .MaximumLength(Url.MaxLength)
                .When(x => !string.IsNullOrEmpty(x.ImageUrl));
        }
    }
}