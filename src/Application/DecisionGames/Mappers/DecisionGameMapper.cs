using DecisionMate.Application.Categories.Mappers;
using DecisionMate.Domain.DecisionGames;
using DecisionMate.Domain.DecisionGames.Models;
using Riok.Mapperly.Abstractions;
using static Riok.Mapperly.Abstractions.RequiredMappingStrategy;

namespace DecisionMate.Application.DecisionGames.Mappers;

[Mapper]
[UseStaticMapper(typeof(OptionMapper))]
public static partial class DecisionGameMapper
{

    [MapperRequiredMapping(Target)]
    public static partial DecisionGameCreatedModel MapToCreatedModel(this DecisionGame entity);  
    
    [MapperRequiredMapping(Target)]
    public static partial DecisionGameModel MapToModel(this DecisionGame entity);
}