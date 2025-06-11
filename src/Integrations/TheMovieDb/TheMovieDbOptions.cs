namespace DecisionMate.Integrations.TheMovieDb;

public record TheMovieDbOptions
{
    public required string Key { get; init; }
    public required string MovieEndpointUrl { get; init; }
    public required string GenreEndpointUrl { get; init; }
}