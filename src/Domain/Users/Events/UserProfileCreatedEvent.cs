using DecisionMate.Domain.Common;
using DecisionMate.Domain.Users.ValueObjects;
using JetBrains.Annotations;

namespace DecisionMate.Domain.Users.Events;

[PublicAPI]
public sealed record UserProfileCreatedEvent(Guid Id, UserName UserName, Url? AvatarUrl) : IDomainEvent;