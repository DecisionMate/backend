using JetBrains.Annotations;

namespace DecisionMate.Domain.Categories;

[PublicAPI]
public sealed record CategoryListModel(
    Guid Id,
    string Name,
    string Description,
    string? ImageUrl
);