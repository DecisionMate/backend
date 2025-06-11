using Ardalis.Result;
using DecisionMate.Domain.Users;
using Geneirodan.Abstractions.Repositories;
using Gridify;

namespace DecisionMate.Application.UserProfiles;

public interface IUserProfileRepository : IRepository<UserProfile, Guid>
{
    Task<Result<PageModel<UserListModel>>> SearchProfilesAsync(
        string username,
        IGridifyPagination query, 
        CancellationToken cancellationToken
        );
}