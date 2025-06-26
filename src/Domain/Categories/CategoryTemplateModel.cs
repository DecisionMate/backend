using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using DecisionMate.Domain.Categories.Enums;
using JetBrains.Annotations;

namespace DecisionMate.Domain.Categories;

[PublicAPI]
public record CategoryTemplateModel(
    string Name,
    string? Description,
    string? ImageUrl,
    CategoryType Type,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    JsonNode? FiltersJson,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    IDictionary<string, object>? Filters
);