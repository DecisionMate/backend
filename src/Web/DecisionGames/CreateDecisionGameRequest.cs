using DecisionMate.Domain.DecisionGames.ValueObjects;

namespace DecisionMate.Web.DecisionGames;

public record CreateDecisionGameRequest(Guid CategoryId, GameSettings Settings);