using DecisionMate.Application.DecisionGames.Commands;
using Riok.Mapperly.Abstractions;
using static Riok.Mapperly.Abstractions.RequiredMappingStrategy;

namespace DecisionMate.Web.DecisionGames;

[Mapper]
internal static partial class DecisionGameRequestsMapper
{
    [MapperRequiredMapping(Both)]
    public static partial CreateGameCommand MapToCommand(this CreateDecisionGameRequest request);
    
    [MapperRequiredMapping(Both)]
    public static partial UpdateGameCommand MapToCommand(this UpdateDecisionGameRequest request, Guid id);
}