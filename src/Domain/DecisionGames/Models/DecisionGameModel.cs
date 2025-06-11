using DecisionMate.Domain.DecisionGames.ValueObjects;
using JetBrains.Annotations;

namespace DecisionMate.Domain.DecisionGames.Models;

[PublicAPI]
public sealed record DecisionGameModel(
    Guid Id,
    Guid CategoryId,
    GameSettings Settings,
    string? Code,
    IReadOnlyCollection<DecisionResult> Results
);