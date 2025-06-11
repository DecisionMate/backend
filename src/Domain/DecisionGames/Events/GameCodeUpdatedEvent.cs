using DecisionMate.Domain.DecisionGames.ValueObjects;
using JetBrains.Annotations;

namespace DecisionMate.Domain.DecisionGames.Events;

[PublicAPI]
public sealed record GameCodeUpdatedEvent(Guid Id, JoinCode? Code) : IDecisionGameEvent;