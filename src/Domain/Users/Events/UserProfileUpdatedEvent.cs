using DecisionMate.Domain.Common;
using DecisionMate.Domain.Users.ValueObjects;

namespace DecisionMate.Domain.Users.Events;

public sealed record UserProfileUpdatedEvent(Guid Id, UserName UserName, Url? AvatarUrl) : IDomainEvent;