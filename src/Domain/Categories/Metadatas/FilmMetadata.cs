using JetBrains.Annotations;

namespace DecisionMate.Domain.Categories.Metadatas;

[PublicAPI]
public record FilmMetadata(
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
) : Metadata;