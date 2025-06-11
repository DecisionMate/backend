using Ardalis.Result;
using DecisionMate.Domain.Categories;
using Geneirodan.MediatR.Abstractions;
using MediatR;

namespace DecisionMate.Application.Categories.Queries;

public sealed record GetCategoryByIdQuery(Guid Id) : IQuery<CategoryModel>
{
    public sealed class Handler(ICategoryRepository repository)
        : IRequestHandler<GetCategoryByIdQuery, Result<CategoryModel>>
    {
        public async Task<Result<CategoryModel>> Handle(GetCategoryByIdQuery request, CancellationToken token) =>
            await repository.GetCategory(request.Id, token).ConfigureAwait(false) is { } model
                ? Result<CategoryModel>.Success(model)
                : Result<CategoryModel>.NotFound();
    }
}