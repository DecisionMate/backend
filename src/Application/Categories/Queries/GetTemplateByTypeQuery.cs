using Ardalis.Result;
using DecisionMate.Application.Categories.Providers;
using DecisionMate.Domain.Categories;
using DecisionMate.Domain.Categories.Enums;
using Geneirodan.MediatR.Abstractions;
using MediatR;

namespace DecisionMate.Application.Categories.Queries;

public sealed record GetTemplateByTypeQuery(CategoryType Type)
    : IQuery<CategoryTemplateModel>
{
    public sealed class Handler(IEnumerable<ICategoryProvider> providers)
        : IRequestHandler<GetTemplateByTypeQuery, Result<CategoryTemplateModel>>
    {
        public async Task<Result<CategoryTemplateModel>> Handle(
            GetTemplateByTypeQuery request,
            CancellationToken cancellationToken
        )
        {
            var categoryProvider = providers.FirstOrDefault(x => x.CategoryType == request.Type);
            if (categoryProvider is null)
                return Result.NotFound();
            return await categoryProvider.GetFullTemplateAsync(cancellationToken).ConfigureAwait(false);
        }
    }
}