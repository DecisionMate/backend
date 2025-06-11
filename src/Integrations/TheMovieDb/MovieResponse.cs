namespace DecisionMate.Integrations.TheMovieDb;

internal record MovieResponse(
    int Page,
    Movie[] Results,
    int TotalPages,
    int TotalResults
);