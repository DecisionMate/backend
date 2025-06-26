using JetBrains.Annotations;

namespace DecisionMate.Integrations.TheMovieDb;

[PublicAPI]
public interface IMovieDbFilters
{
    string? WithGenres { get; }
    string? WithoutGenres { get; }
    string? WithCountries { get; }
    string? WithoutCountries { get; }
    int? StartYear { get; }
    int? EndYear { get; }

    public static IDictionary<string, string> Mappings() =>
        new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "start_year", "release_date.gte" },
            { "end_year", "release_date.lte" }
        };
}