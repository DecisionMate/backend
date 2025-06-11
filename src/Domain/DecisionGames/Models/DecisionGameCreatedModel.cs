using DecisionMate.Domain.DecisionGames.ValueObjects;
using JetBrains.Annotations;

namespace DecisionMate.Domain.DecisionGames.Models;

[PublicAPI]
public sealed record DecisionGameCreatedModel(
    Guid Id,
    Guid CategoryId,
    GameSettings Settings
);