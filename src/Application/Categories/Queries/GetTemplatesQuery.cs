using Ardalis.Result;
using DecisionMate.Application.Categories.Providers;
using DecisionMate.Domain.Categories;
using Geneirodan.MediatR.Abstractions;
using MediatR;

namespace DecisionMate.Application.Categories.Queries;

public sealed record GetTemplatesQuery
    : IQuery<IReadOnlyCollection<CategoryTemplateModel>>
{
    public sealed class Handler(IEnumerable<ICategoryProvider> providers)
        : IRequestHandler<GetTemplatesQuery, Result<IReadOnlyCollection<CategoryTemplateModel>>>
    {
        public Task<Result<IReadOnlyCollection<CategoryTemplateModel>>> Handle(
            GetTemplatesQuery request,
            CancellationToken cancellationToken
        ) => Task.FromResult<Result<IReadOnlyCollection<CategoryTemplateModel>>>(
            providers.Select(provider => provider.GetShortTemplate()).ToArray()
        );
    }
}