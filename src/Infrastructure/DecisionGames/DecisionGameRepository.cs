using DecisionMate.Application.DecisionGames;
using DecisionMate.Domain.DecisionGames;
using DecisionMate.Domain.DecisionGames.Models;
using Geneirodan.Abstractions.Mapping;
using Geneirodan.EntityFrameworkCore;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace DecisionMate.Infrastructure.DecisionGames;

[UsedImplicitly]
public sealed class DecisionGameRepository(
    DbContext context,
    IEntityMapper<DecisionGame, DecisionGameModel> modelMapper
) : Repository<DecisionGame, Guid>(context),
    IDecisionGameRepository
{
    public override Task<DecisionGame?> FindAsync(Guid id, CancellationToken token = default) => 
        base.FindAsync(Set.Include(x => x.Players), id, token);

    public async Task<DecisionGameModel?> GetByIdAsync(Guid id, CancellationToken token) =>
        await Set.AsNoTracking()
            .Where(x => x.Id.Equals(id))
            .ProjectTo<DecisionGame, DecisionGameModel>(modelMapper)
            .SingleOrDefaultAsync(token)
            .ConfigureAwait(false);
}