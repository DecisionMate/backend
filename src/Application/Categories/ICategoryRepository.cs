using Ardalis.Result;
using DecisionMate.Domain.Categories;
using Geneirodan.Abstractions.Repositories;
using Gridify;

namespace DecisionMate.Application.Categories;

public interface ICategoryRepository : IRepository<Category, Guid>
{
    Task<Result<PageModel<CategoryListModel>>> GetCategories(GridifyQuery query, CancellationToken cancellationToken);
    Task<CategoryModel?> GetCategory(Guid id, CancellationToken cancellationToken);
}