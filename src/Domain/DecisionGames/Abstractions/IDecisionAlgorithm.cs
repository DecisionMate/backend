using DecisionMate.Domain.DecisionGames.ValueObjects;

namespace DecisionMate.Domain.DecisionGames.Abstractions;

public interface IDecisionAlgorithm
{
    public DecisionResult[] Execute(params IReadOnlyCollection<DecisionGame.Player.IChoices> choices);
}