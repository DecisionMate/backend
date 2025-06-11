using DecisionMate.Domain.DecisionGames;
using DecisionMate.Domain.DecisionGames.Abstractions;
using DecisionMate.Domain.DecisionGames.ValueObjects;

namespace DecisionMate.Application.DecisionGames.Services.Algorithms;

public sealed class SimpsonAlgorithm : IDecisionAlgorithm
{

    public DecisionResult[] Execute(params IReadOnlyCollection<DecisionGame.Player.IChoices> choices)
    {
        var pairs = new Dictionary<(int, int), int>();
        foreach (var top in choices)
            for (var i = 0; i < top.Size; i++)
            for (var j = i + 1; j < top.Size; j++)
            {
                var key = (top[i], top[j]);
                pairs.TryAdd(key, 0);
                pairs[key]++;
            }

        var points = new Dictionary<int, int>();
        foreach (var ((item1, _), value) in pairs)
            if (!points.TryAdd(item1, value) && points[item1] > value) 
                points[item1] = value;

        return points.Select(x => new DecisionResult(x.Key, x.Value)).OrderByDescending(x => x.Points).ToArray();
    }
}