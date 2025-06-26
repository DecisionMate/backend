using DecisionMate.Domain.Common;
using JetBrains.Annotations;

namespace DecisionMate.Domain.Users.Events;

[PublicAPI]
public sealed record UserProfileDeletedEvent(Guid Id) : IDomainEvent;