namespace DecisionMate.Domain.Categories.Metadatas;

public record FilmMetadata(
    bool adult,
    string BackdropPath,
    int[] GenreIds,
    int id,
    string OriginalLanguage,
    string OriginalTitle,
    string overview,
    double popularity,
    string PosterPath,
    string ReleaseDate,
    string title,
    bool video,
    double VoteAverage,
    int VoteCount
) : Metadata;