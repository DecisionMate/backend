using DecisionMate.Domain.Categories.ValueObjects;
using DecisionMate.Domain.Common;

namespace DecisionMate.Domain.Categories;

public record CategoryUpdatedEvent(
    Guid Id,
    CategoryName Name,
    CategoryDescription? Description,
    Url? ImageUrl
) : IDomainEvent;