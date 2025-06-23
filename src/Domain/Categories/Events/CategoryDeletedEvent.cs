using DecisionMate.Domain.Common;

namespace DecisionMate.Domain.Categories;

public record CategoryDeletedEvent(Guid Id) : IDomainEvent;