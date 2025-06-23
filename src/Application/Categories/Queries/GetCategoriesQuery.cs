using Ardalis.Result;
using DecisionMate.Application.Common;
using DecisionMate.Domain.Categories;
using Geneirodan.Abstractions.Repositories;
using MediatR;

namespace DecisionMate.Application.Categories.Queries;

public sealed record GetCategoriesQuery(string SearchTerm, IPagination Pagination) : IPaginatedQuery<CategoryListModel>
{
    public sealed class Handler(ICategoryRepository repository)
        : IRequestHandler<GetCategoriesQuery, Result<PageModel<CategoryListModel>>>
    {
        public async Task<Result<PageModel<CategoryListModel>>> Handle(
            GetCategoriesQuery request,
            CancellationToken cancellationToken
        ) => await repository.GetCategories(request.SearchTerm, request.Pagination, cancellationToken)
            .ConfigureAwait(false);
    }
}

// public sealed record GetPresetCategoriesQuery() 
//     : IQuery<IReadOnlyCollection<CategoryListModel>>
// {
//     public sealed class Handler(ICategoryRepository repository)
//         : IRequestHandler<GetPresetCategoriesQuery, Result<IReadOnlyCollection<CategoryListModel>>>
//     {
//         public async Task<Result<IReadOnlyCollection<CategoryListModel>>> Handle(
//             GetPresetCategoriesQuery request, CancellationToken cancellationToken
//         )
//         {
//             
//         }
//     }
// }