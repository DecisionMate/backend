using DecisionMate.Domain.DecisionGames.Models;
using JetBrains.Annotations;
using Riok.Mapperly.Abstractions;
using DecisionGame = DecisionMate.Domain.DecisionGames.DecisionGame;

namespace DecisionMate.Infrastructure.DecisionGames;

[Mapper]
public static partial class DecisionGameModelMappingExtensions
{
    [MapperRequiredMapping(RequiredMappingStrategy.Target)]
    [UsedImplicitly]
    public static partial DecisionGameModel Map(this DecisionGame source);

    public static partial IQueryable<DecisionGameModel> ProjectToModels(this IQueryable<DecisionGame> source);
}