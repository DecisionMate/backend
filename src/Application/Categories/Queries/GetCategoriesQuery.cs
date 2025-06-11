using Ardalis.Result;
using DecisionMate.Application.Common;
using DecisionMate.Domain.Categories;
using FluentValidation;
using Geneirodan.Abstractions.Repositories;
using Geneirodan.MediatR.Abstractions;
using Gridify;
using JetBrains.Annotations;
using MediatR;

namespace DecisionMate.Application.Categories.Queries;

public sealed record GetCategoriesQuery(GridifyQuery Query) : IQuery<PageModel<CategoryListModel>>
{
    public sealed class Handler(ICategoryRepository repository)
        : IRequestHandler<GetCategoriesQuery, Result<PageModel<CategoryListModel>>>
    {
        public async Task<Result<PageModel<CategoryListModel>>> Handle(
            GetCategoriesQuery request,
            CancellationToken cancellationToken
        ) => await repository.GetCategories(request.Query, cancellationToken).ConfigureAwait(false);
    }

    [UsedImplicitly]
    public sealed class Validator : AbstractValidator<GetCategoriesQuery>
    {
        public Validator(IGridifyValidator<Category> gridifyValidator) =>
            RuleFor(x => x.Query).SetValidator(gridifyValidator);
    }
}