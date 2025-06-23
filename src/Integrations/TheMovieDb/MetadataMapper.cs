using DecisionMate.Domain.Categories.Metadatas;
using Riok.Mapperly.Abstractions;

namespace DecisionMate.Integrations.TheMovieDb;

[Mapper]
public static partial class MetadataMapper
{
    public static partial FilmMetadata MapToMetadata(this Movie movie);
}