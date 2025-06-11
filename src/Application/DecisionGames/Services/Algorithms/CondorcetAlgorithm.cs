using DecisionMate.Domain.DecisionGames;
using DecisionMate.Domain.DecisionGames.Abstractions;
using DecisionMate.Domain.DecisionGames.ValueObjects;

namespace DecisionMate.Application.DecisionGames.Services.Algorithms;

public sealed class CondorcetAlgorithm : IDecisionAlgorithm
{
    public DecisionResult[] Execute(params IReadOnlyCollection<DecisionGame.Player.IChoices> choices)
    {
        var pairs = new Dictionary<(int, int), int>();
        foreach (var top in choices)
            for (var i = 0; i < top.Size; i++)
            for (var j = i + 1; j < top.Size; j++)
            {
                if (pairs.ContainsKey((top[i], top[j])))
                    pairs[(top[i], top[j])]++;
                else
                {
                    var key = (top[j], top[i]);
                    pairs.TryAdd(key, 0);
                    pairs[key]--;
                }
            }

        var points = new Dictionary<int, int>();
        foreach (var ((item1, item2), value) in pairs)
            switch (value)
            {
                case > 0 when !points.TryAdd(item1, 1):
                    points[item1]++;
                    break;
                case < 0 when !points.TryAdd(item2, 1):
                    points[item2]++;
                    break;
            }

        return points.Select(x => new DecisionResult(x.Key, x.Value))
            .OrderByDescending(x => x.Points)
            .ToArray();
    }
}