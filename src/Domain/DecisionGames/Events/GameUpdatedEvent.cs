using DecisionMate.Domain.DecisionGames.ValueObjects;
using JetBrains.Annotations;

namespace DecisionMate.Domain.DecisionGames.Events;

[PublicAPI]
public sealed record GameUpdatedEvent(Guid Id, GameSettings Settings, JoinCode? Code) : IDecisionGameEvent;