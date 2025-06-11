using DecisionMate.Application.Common.Resources;
using DecisionMate.Domain.DecisionGames;
using DecisionMate.Domain.DecisionGames.Abstractions;
using DecisionMate.Domain.DecisionGames.ValueObjects;

namespace DecisionMate.Application.DecisionGames.Services.Algorithms;

public sealed class BordaAlgorithm : IDecisionAlgorithm
{
    public DecisionResult[] Execute(
        params IReadOnlyCollection<DecisionGame.Player.IChoices> choices)
    {
        var length = choices.FirstOrDefault()?.Size;

        if (length is null)
            return [];

        if (choices.Any(x => x.Size != length))
            throw new ArgumentException(ErrorMessages.AllTopsMustHaveTheSameLength, nameof(choices));

        return choices
            .SelectMany(x => x.Select((c, i) => (Option: c, Place: i)))
            .GroupBy(x => x.Option)
            .Select(x => new DecisionResult(x.Key, x.Aggregate(0, (i, tuple) => i + length.Value - tuple.Place - 1)))
            .OrderByDescending(x => x.Points)
            .ToArray();
    }
}