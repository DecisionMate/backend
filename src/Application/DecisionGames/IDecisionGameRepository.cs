using DecisionMate.Domain.DecisionGames;
using DecisionMate.Domain.DecisionGames.Models;
using Geneirodan.Abstractions.Repositories;

namespace DecisionMate.Application.DecisionGames;

public interface IDecisionGameRepository : IRepository<DecisionGame, Guid>
{
    Task<DecisionGameModel?> GetByIdAsync(Guid id, CancellationToken token);
}