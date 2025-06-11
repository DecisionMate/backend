using DecisionMate.Domain.DecisionGames.ValueObjects;
using JetBrains.Annotations;

namespace DecisionMate.Domain.DecisionGames.Events;

[PublicAPI]
public sealed record GameCreatedEvent(Guid Id, Guid CategoryId, GameSettings Settings) : IDecisionGameEvent;