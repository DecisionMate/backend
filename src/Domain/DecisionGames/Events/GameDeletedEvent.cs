using JetBrains.Annotations;

namespace DecisionMate.Domain.DecisionGames.Events;

[PublicAPI]
public sealed record GameDeletedEvent(Guid Id) : IDecisionGameEvent;