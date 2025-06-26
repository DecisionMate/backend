using JetBrains.Annotations;

namespace DecisionMate.Integrations.TheMovieDb;

[PublicAPI]
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