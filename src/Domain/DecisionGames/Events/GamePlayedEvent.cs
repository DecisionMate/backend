using DecisionMate.Domain.DecisionGames.ValueObjects;
using JetBrains.Annotations;

namespace DecisionMate.Domain.DecisionGames.Events;

[PublicAPI]
public sealed record GamePlayedEvent(Guid Id, IReadOnlyCollection<DecisionResult> Results) : IDecisionGameEvent;