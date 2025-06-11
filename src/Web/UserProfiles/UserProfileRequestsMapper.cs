using DecisionMate.Application.UserProfiles.Commands;
using Riok.Mapperly.Abstractions;

namespace DecisionMate.Web.UserProfiles;

[Mapper]
internal static partial class UserProfileRequestsMapper
{
    [MapperRequiredMapping(RequiredMappingStrategy.Target)]
    public static partial AddUserProfileCommand MapToCommand(this AddUserProfileRequest request);
}