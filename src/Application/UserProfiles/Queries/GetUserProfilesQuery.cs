using Ardalis.Result;
using DecisionMate.Application.Common;
using DecisionMate.Domain.Users;
using Geneirodan.Abstractions.Repositories;
using MediatR;

namespace DecisionMate.Application.UserProfiles.Queries;

public sealed record GetUserProfilesQuery(string Username, IPagination Pagination) : IPaginatedQuery<UserListModel>
{
    public sealed class Handler(IUserProfileRepository repository)
        : IRequestHandler<GetUserProfilesQuery, Result<PageModel<UserListModel>>>
    {
        public async Task<Result<PageModel<UserListModel>>> Handle(
            GetUserProfilesQuery request,
            CancellationToken cancellationToken
        ) => await repository.SearchProfilesAsync(request.Username, request.Pagination, cancellationToken).ConfigureAwait(false);
    }
}