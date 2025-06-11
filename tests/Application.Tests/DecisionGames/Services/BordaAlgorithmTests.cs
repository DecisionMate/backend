using DecisionMate.Application.DecisionGames.Services.Algorithms;
using DecisionMate.Domain.DecisionGames.ValueObjects;
using JetBrains.Annotations;
using Shouldly;
using static DecisionMate.Domain.DecisionGames.DecisionGame.Player;

namespace DecisionMate.Application.Tests.DecisionGames.Services;

[TestSubject(typeof(BordaAlgorithm))]
public sealed class BordaAlgorithmTests
{
    private readonly BordaAlgorithm _algorithm = new();

    [Theory, MemberData(nameof(TestDataGenerator))]
    public void Execute_ShouldReturnExpectedResults(IReadOnlyCollection<IChoices> choices, DecisionResult[] expected)
    {
        var results = _algorithm.Execute(choices);
        results.ToArray().ShouldBeEquivalentTo(expected);
    }

    [Fact]
    public void Execute_ShouldThrowException_WhenTopsLengthAreDifferent()
    {
        IChoices[] choices = [[1,2], [1]];
        var exception = Should.Throw<ArgumentException>(() => _algorithm.Execute(choices));
        exception.ParamName.ShouldBe("choices");
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
                [(4, 29), (3, 27), (1, 24), (2, 22)]
            },
            {
                [],[]
            }
        };
}