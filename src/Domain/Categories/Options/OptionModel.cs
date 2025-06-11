using DecisionMate.Domain.Categories.Metadatas;
using JetBrains.Annotations;

namespace DecisionMate.Domain.Categories.Options;

[PublicAPI]
public sealed record OptionModel(string Name, string Description, Metadata? Metadata, string? ImageUrl);