using JetBrains.Annotations;

namespace DecisionMate.Domain.Categories.Metadatas;

[PublicAPI]
public record GameMetadata(int Genre, string Title) : Metadata;