namespace DecisionMate.Integrations.TheMovieDb;

public record Movie(
    bool Adult,
    string BackdropPath,
    int[] GenreIds,
    int Id,
    string OriginalLanguage,
    string OriginalTitle,
    string Overview,
    double Popularity,
    string PosterPath,
    string ReleaseDate,
    string Title,
    bool Video,
    double VoteAverage,
    int VoteCount
);