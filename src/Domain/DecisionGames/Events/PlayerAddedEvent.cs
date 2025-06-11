using JetBrains.Annotations;

namespace DecisionMate.Domain.DecisionGames.Events;

[PublicAPI]
public sealed record PlayerAddedEvent(Guid Id, DecisionGame.Player Player) : IDecisionGameEvent;