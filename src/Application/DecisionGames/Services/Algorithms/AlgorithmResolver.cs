using DecisionMate.Domain.DecisionGames.Abstractions;
using DecisionMate.Domain.DecisionGames.Enums;
using JetBrains.Annotations;

namespace DecisionMate.Application.DecisionGames.Services.Algorithms;

[UsedImplicitly]
public sealed class AlgorithmResolver : IAlgorithmResolver
{
    public IDecisionAlgorithm GetAlgorithm(Algorithm algorithm) =>
        algorithm switch {
            Algorithm.Condorcet => new CondorcetAlgorithm(),
            Algorithm.Borda => new BordaAlgorithm(),
            Algorithm.Simpson => new SimpsonAlgorithm(),
            _ => throw new ArgumentOutOfRangeException(nameof(algorithm), algorithm, message: null)
        };
}