using DecisionMate.Application.DecisionGames.Services.Algorithms;
using DecisionMate.Domain.DecisionGames.ValueObjects;
using JetBrains.Annotations;
using Shouldly;
using static DecisionMate.Domain.DecisionGames.DecisionGame.Player;

namespace DecisionMate.Application.Tests.DecisionGames.Services;

[TestSubject(typeof(SimpsonAlgorithm))]
public sealed class SimpsonAlgorithmTests
{
    private readonly SimpsonAlgorithm _algorithm = new();

    [Theory, MemberData(nameof(TestDataGenerator))]
    public void Execute_ShouldReturnExpectedResults(IReadOnlyCollection<IChoices> choices, DecisionResult[][] expected)
    {
        var results = _algorithm.Execute(choices);
        results.ToArray().ShouldBeOneOf(expected, new ArrayComparer());
    }

    public static TheoryData<IReadOnlyCollection<IChoices>, DecisionResult[][]> TestDataGenerator() =>
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
                [[(3, 9), (1, 8), (4, 8), (2, 5)], [(3, 9), (4, 8), (1, 8), (2, 5)]]
            },
            {
                [],[[]]
            }
        };

    private sealed class ArrayComparer : EqualityComparer<DecisionResult[]>
    {
        public override bool Equals(DecisionResult[]? x, DecisionResult[]? y) =>
            x is null && y is null
            || x is not null && y is not null && x.Length == y.Length && !x.Where((t, i) => t != y[i]).Any();

        public override int GetHashCode(DecisionResult[] obj) =>
            obj.Aggregate(0, (current, i) => current ^ i.GetHashCode());
    }
}