namespace DecisionMate.Integrations.TheMovieDb;

public record TheMovieDbOptions
{
    public required string ApiKey { get; init; }
    public required string MovieEndpointUrl { get; init; }
    public required string GenreEndpointUrl { get; init; }
    public static int PageSize => 20;
    public required string CategoryName { get; init; }
    public string? CategoryDescription { get; init; }
    public string? CategoryImageUrl { get; init; }
}