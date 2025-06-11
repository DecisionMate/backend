using DecisionMate.Domain.DecisionGames.ValueObjects;

namespace DecisionMate.Web.DecisionGames;

internal record UpdateDecisionGameRequest(GameSettings Settings, string? ImageUrl);