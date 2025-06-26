using Ardalis.Result;
using DecisionMate.Application.Categories.Providers;
using DecisionMate.Domain.Categories;
using DecisionMate.Domain.Categories.Enums;
using Geneirodan.MediatR.Abstractions;
using MediatR;

namespace DecisionMate.Application.Categories.Queries;

public sealed record GetPremadeCategoryByTypeQuery(CategoryType type, int count, IDictionary<string, string> filters)
    : IQuery<CategoryModel>
{
    public sealed class Handler(IEnumerable<ICategoryProvider> providers)
        : IRequestHandler<GetPremadeCategoryByTypeQuery, Result<CategoryModel>>
    {
        public async Task<Result<CategoryModel>> Handle(
            GetPremadeCategoryByTypeQuery request, CancellationToken cancellationToken
        )
        {
            var (type, count, filters) = request;
            var categoryProvider = providers.FirstOrDefault(x => x.CategoryType == type);
            if (categoryProvider is null)
                return Result.NotFound();
            return await categoryProvider.GetCategoryAsync(count, filters, cancellationToken).ConfigureAwait(false);
        }
    }
}