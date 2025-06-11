using JetBrains.Annotations;

namespace DecisionMate.Domain.DecisionGames.Events;

[PublicAPI]
public sealed record PlayerChoicesSetEvent(Guid Id, Guid PlayerId, DecisionGame.Player.IChoices choices) : IDecisionGameEvent;