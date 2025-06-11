using DecisionMate.Domain.DecisionGames.Enums;

namespace DecisionMate.Domain.DecisionGames.Abstractions;

public interface IAlgorithmResolver
{
    IDecisionAlgorithm GetAlgorithm(Algorithm algorithm);
}