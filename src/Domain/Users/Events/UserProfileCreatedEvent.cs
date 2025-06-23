using DecisionMate.Domain.Common;
using DecisionMate.Domain.Users.ValueObjects;

namespace DecisionMate.Domain.Users;

public sealed record UserProfileCreatedEvent(Guid Id, UserName UserName, Url? AvatarUrl) : IDomainEvent;