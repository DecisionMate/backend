using DecisionMate.Domain.DecisionGames.Models;
using Geneirodan.Abstractions.Mapping;
using JetBrains.Annotations;
using Riok.Mapperly.Abstractions;

namespace DecisionMate.Infrastructure.DecisionGames;

[Mapper, UsedImplicitly]
public sealed partial class DecisionGameModelMapper : IEntityMapper<Domain.DecisionGames.DecisionGame, DecisionGameModel>
{
    [MapperRequiredMapping(RequiredMappingStrategy.Target)]
    public partial DecisionGameModel Map(Domain.DecisionGames.DecisionGame source);

    public partial IQueryable<DecisionGameModel> Map(IQueryable<Domain.DecisionGames.DecisionGame> source);
}