using JetBrains.Annotations;

namespace DecisionMate.Domain.DecisionGames.Events;

[PublicAPI]
public sealed record PlayerRemovedEvent(Guid Id, Guid PlayerId) : IDecisionGameEvent;