using DecisionMate.Application.Categories.Contracts;

namespace DecisionMate.Web.Categories;

internal record AddCategoryRequest(
    string Name,
    string Description,
    IEnumerable<OptionContract> Options,
    string? ImageUrl
);