using Ardalis.Result;
using DecisionMate.Domain.Categories;
using DecisionMate.Integrations.TheMovieDb;

namespace DecisionMate.Integrations;

public interface IMoviesCategoryProvider
{
    ValueTask<IReadOnlyCollection<Genre>?> GetGenresAsync(
        string lang = "en",
        CancellationToken cancellationToken = default
    );

    Task<Result<CategoryModel>> GetCategoryAsync(
        int count,
        string? withGenres = null,
        string? withoutGenres = null,
        string? withCountries = null,
        string? withoutCountries = null,
        int? startYear = null,
        int? endYear = null,
        CancellationToken cancellationToken = default
    );
}