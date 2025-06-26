using Ardalis.Result;
using DecisionMate.Domain.Categories;
using DecisionMate.Domain.Categories.Enums;

namespace DecisionMate.Application.Categories.Providers;

public abstract class MovieCategoryProvider : ICategoryProvider
{
    public CategoryType CategoryType => CategoryType.Movies;

    public abstract Task<CategoryTemplateModel> GetFullTemplateAsync(CancellationToken cancellationToken);

    public virtual CategoryTemplateModel GetShortTemplate() => new(
        Name: Common.Resources.Messages.MoviesCategoryName,
        Description: null,
        ImageUrl: null,
        Type: CategoryType,
        FiltersJson: null,
        Filters: null
    );

    public abstract Task<Result<CategoryModel>> GetCategoryAsync(
        int count,
        IDictionary<string, string> filters,
        CancellationToken cancellationToken = default
    );
}