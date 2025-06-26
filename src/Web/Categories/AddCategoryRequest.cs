using DecisionMate.Application.Categories.Contracts;
using DecisionMate.Domain.Categories.Enums;

namespace DecisionMate.Web.Categories;

internal record AddCategoryRequest(
    string Name,
    string Description,
    CategoryType Type,
    IEnumerable<OptionContract> Options,
    string? ImageUrl
);