using DecisionMate.Domain.Common;

namespace DecisionMate.Domain.DecisionGames.Events;

public interface IDecisionGameEvent : IDomainEvent
{
    Guid Id { get; }
}