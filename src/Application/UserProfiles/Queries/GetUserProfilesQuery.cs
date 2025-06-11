using Ardalis.Result;
using DecisionMate.Domain.Users;
using Geneirodan.Abstractions.Repositories;
using Geneirodan.MediatR.Abstractions;
using Gridify;
using MediatR;

namespace DecisionMate.Application.UserProfiles.Queries;

public sealed record GetUserProfilesQuery(string username, IGridifyPagination Query) : IQuery<PageModel<UserListModel>>
{
    public sealed class Handler(IUserProfileRepository repository)
        : IRequestHandler<GetUserProfilesQuery, Result<PageModel<UserListModel>>>
    {
        public async Task<Result<PageModel<UserListModel>>> Handle(
            GetUserProfilesQuery request,
            CancellationToken cancellationToken
        ) => await repository.SearchProfilesAsync(request.username, request.Query, cancellationToken).ConfigureAwait(false);
    }
}