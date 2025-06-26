using Ardalis.Result;
using DecisionMate.Application.Common;
using DecisionMate.Domain.Categories;
using DecisionMate.Domain.Categories.Enums;
using Geneirodan.Abstractions.Repositories;

namespace DecisionMate.Application.Categories;

public interface ICategoryRepository : IRepository<Category, Guid>
{
    Task<Result<PageModel<CategoryListModel>>> GetCategories(string searchTerm,
        IPagination pagination,
        CategoryType type,
        CancellationToken cancellationToken);

    Task<CategoryModel?> GetCategory(Guid id, CancellationToken cancellationToken);
}