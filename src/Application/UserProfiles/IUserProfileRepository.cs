using Ardalis.Result;
using DecisionMate.Application.Common;
using DecisionMate.Domain.Users;
using Geneirodan.Abstractions.Repositories;

namespace DecisionMate.Application.UserProfiles;

public interface IUserProfileRepository : IRepository<UserProfile, Guid>
{
    Task<Result<PageModel<UserListModel>>> SearchProfilesAsync(
        string username,
        IPagination query, 
        CancellationToken cancellationToken
        );
}