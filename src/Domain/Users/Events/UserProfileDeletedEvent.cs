using DecisionMate.Domain.Common;

namespace DecisionMate.Domain.Users;

public sealed record UserProfileDeletedEvent(Guid Id) : IDomainEvent;