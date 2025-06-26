using System.Text.Json.Serialization;

namespace DecisionMate.Integrations.TheMovieDb;

internal record MovieResponse
{
    public int Page { get; init; }
    public required Movie[] Results { get; init; }
    public int TotalPages { get; init; }
    public int TotalResults { get; init; }

}