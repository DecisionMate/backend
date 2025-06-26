using Ardalis.Result;
using DecisionMate.Domain.Categories;
using DecisionMate.Domain.Categories.Enums;

namespace DecisionMate.Application.Categories.Providers;

public interface ICategoryProvider
{
    CategoryType  CategoryType { get; }
    
    Task<CategoryTemplateModel> GetFullTemplateAsync(CancellationToken cancellationToken);
    CategoryTemplateModel GetShortTemplate();
    
    Task<Result<CategoryModel>> GetCategoryAsync(
        int count,
        IDictionary<string, string> filters,
        CancellationToken cancellationToken = default
    );
}