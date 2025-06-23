using System.Text.Json.Serialization;

namespace DecisionMate.Integrations.TheMovieDb;

internal record MovieResponse
{
    public int Page { get; init; }
    public required Movie[] Results { get; init; }
    [JsonPropertyName("total_pages")]
    public int TotalPages { get; init; }
    [JsonPropertyName("total_results")]
    public int TotalResults { get; init; }

}