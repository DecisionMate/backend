using DecisionMate.Domain.Categories.ValueObjects;
using DecisionMate.Domain.Common;
using JetBrains.Annotations;

namespace DecisionMate.Domain.Categories.Events;

[PublicAPI]
public record CategoryUpdatedEvent(
    Guid Id,
    CategoryName Name,
    CategoryDescription? Description,
    Url? ImageUrl
) : IDomainEvent;