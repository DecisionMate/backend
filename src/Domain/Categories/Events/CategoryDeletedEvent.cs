using DecisionMate.Domain.Common;
using JetBrains.Annotations;

namespace DecisionMate.Domain.Categories.Events;

[PublicAPI]
public record CategoryDeletedEvent(Guid Id) : IDomainEvent;