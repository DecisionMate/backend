using DecisionMate.Domain.Categories.Options;
using JetBrains.Annotations;

namespace DecisionMate.Domain.Categories;

[PublicAPI]
public sealed record CategoryModel(
    Guid Id,
    string Name,
    string Description,
    string? ImageUrl,
    OptionModel[] Options
);