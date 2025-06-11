using DecisionMate.Application.DecisionGames.Services.Algorithms;
using DecisionMate.Domain.DecisionGames.ValueObjects;
using JetBrains.Annotations;
using Shouldly;
using static DecisionMate.Domain.DecisionGames.DecisionGame.Player;

namespace DecisionMate.Application.Tests.DecisionGames.Services;

[TestSubject(typeof(CondorcetAlgorithm))]
public sealed class CondorcetAlgorithmTests
{
    private readonly CondorcetAlgorithm _algorithm = new();

    [Theory, MemberData(nameof(TestDataGenerator))]
    public void Execute_ShouldReturnExpectedResults(IReadOnlyCollection<IChoices> choices, DecisionResult[] expected)
    {
        var results = _algorithm.Execute(choices);
        results.ToArray().ShouldBeEquivalentTo(expected);
    }

    public static TheoryData<IReadOnlyCollection<IChoices>, DecisionResult[]> TestDataGenerator() =>
        new()
        {
            {
                [
                    [1, 4, 3, 2],
                    [1, 4, 3, 2],
                    [1, 4, 3, 2],
                    [1, 4, 3, 2],
                    [1, 4, 3, 2],

                    [1, 4, 2, 3],
                    [1, 4, 2, 3],
                    [1, 4, 2, 3],

                    [2, 3, 4, 1],
                    [2, 3, 4, 1],
                    [2, 3, 4, 1],
                    [2, 3, 4, 1],
                    [2, 3, 4, 1],

                    [3, 4, 2, 1],
                    [3, 4, 2, 1],
                    [3, 4, 2, 1],
                    [3, 4, 2, 1]
                ],
                [(3, 3), (4, 2), (2, 1)]
            },
            {
                [],[]
            }
        };
}