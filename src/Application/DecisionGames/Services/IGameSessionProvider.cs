using DecisionMate.Domain.DecisionGames.ValueObjects;

namespace DecisionMate.Application.DecisionGames.Services;

public interface IGameSessionProvider
{
    Task<JoinCode> CreateCodeAsync(Guid gameId);
    Task<Guid?> GetGameSessionByCodeAsync(JoinCode code);
    Task RemoveCodeAsync(JoinCode code);
}