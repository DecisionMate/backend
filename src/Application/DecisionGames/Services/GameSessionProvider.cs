using System.Diagnostics.Contracts;
using DecisionMate.Application.Common;
using DecisionMate.Domain.DecisionGames.ValueObjects;
using Microsoft.Extensions.Caching.Hybrid;
using static DecisionMate.Domain.DecisionGames.ValueObjects.JoinCode;

namespace DecisionMate.Application.DecisionGames.Services;

public sealed class GameSessionProvider(HybridCache cache) : IGameSessionProvider
{
    private const string Symbols = "1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZ";

    public async Task<JoinCode> CreateCodeAsync(Guid gameId)
    {
        JoinCode code;
        do
        {
            var builder = new char[CodeLength];
            for (var i = 0; i < CodeLength; i++)
                builder[i] = Symbols[Random.Shared.Next(0, Symbols.Length)];
            code = new JoinCode(new string(builder));
        } while (await cache.GetAsync<string>(GetKey(code)).ConfigureAwait(false) is not null);

        await cache.SetAsync(GetKey(code), gameId.ToString()).ConfigureAwait(false);
        return code;
    }

    [Pure]
    private static string GetKey(JoinCode code) => $"Code-{code}";

    public async Task<Guid?> GetGameSessionByCodeAsync(JoinCode code)
    {
        var str = await cache.GetAsync<string>(GetKey(code)).ConfigureAwait(false);
        return str is null ? null : Guid.Parse(str);
    }

    public async Task<JoinCode> GetOrCreateCodeAsync(Guid gameId, JoinCode? oldCode) =>
        oldCode is not null &&
        gameId == await GetGameSessionByCodeAsync(oldCode.Value).ConfigureAwait(false)
            ? oldCode.Value
            : await CreateCodeAsync(gameId).ConfigureAwait(false);

    public async Task RemoveCodeAsync(JoinCode code) =>
        await cache.RemoveAsync(GetKey(code)).ConfigureAwait(false);
}