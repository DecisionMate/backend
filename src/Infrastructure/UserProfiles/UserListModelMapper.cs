using DecisionMate.Domain.Users;
using Geneirodan.Abstractions.Mapping;
using JetBrains.Annotations;
using Riok.Mapperly.Abstractions;

namespace DecisionMate.Infrastructure.UserProfiles;

[Mapper, UsedImplicitly]
public sealed partial class UserListModelMapper : IEntityMapper<UserProfile, UserListModel>
{
    [MapperRequiredMapping(RequiredMappingStrategy.Target)]
    public partial UserListModel Map(UserProfile source);

    public partial IQueryable<UserListModel> Map(IQueryable<UserProfile> source);
}