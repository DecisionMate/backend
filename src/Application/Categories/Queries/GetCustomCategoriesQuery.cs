using Ardalis.Result;
using DecisionMate.Application.Common;
using DecisionMate.Domain.Categories;
using DecisionMate.Domain.Categories.Enums;
using Geneirodan.Abstractions.Repositories;
using MediatR;

namespace DecisionMate.Application.Categories.Queries;

public sealed record GetCustomCategoriesQuery(string SearchTerm, IPagination Pagination)
    : IPaginatedQuery<CategoryListModel>
{
    public sealed class Handler(ICategoryRepository repository)
        : IRequestHandler<GetCustomCategoriesQuery, Result<PageModel<CategoryListModel>>>
    {
        public async Task<Result<PageModel<CategoryListModel>>> Handle(
            GetCustomCategoriesQuery request,
            CancellationToken cancellationToken
        ) => await repository
            .GetCategories(request.SearchTerm, request.Pagination, CategoryType.Custom, cancellationToken)
            .ConfigureAwait(false);
    }
}