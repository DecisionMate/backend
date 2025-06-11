using System.Text.Json.Serialization;
using JetBrains.Annotations;

namespace DecisionMate.Domain.Categories.Metadatas;

[PublicAPI]
[JsonPolymorphic]
[JsonDerivedType(typeof(FilmMetadata), nameof(FilmMetadata))]
[JsonDerivedType(typeof(GameMetadata), nameof(GameMetadata))]
public abstract record Metadata;