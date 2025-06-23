using Ardalis.Result;
using Dapper;
using DecisionMate.Application.Common;
using DecisionMate.Application.UserProfiles;
using DecisionMate.Domain.Users;
using Geneirodan.Abstractions.Repositories;
using Geneirodan.EntityFrameworkCore;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace DecisionMate.Infrastructure.UserProfiles;

[UsedImplicitly]
public sealed class UserProfileRepository(DbContext context)
    : Repository<UserProfile, Guid>(context), IUserProfileRepository
{
    private readonly DbContext _context = context;

    public async Task<Result<PageModel<UserListModel>>> SearchProfilesAsync(
        string username,
        IPagination pagination,
        CancellationToken cancellationToken
    )
    {
        var dbConnection = _context.Database.GetDbConnection();
        var command = CreateSearchCommand(username, pagination, cancellationToken);
        var models = await dbConnection.QueryAsync<UserListModel>(command).ConfigureAwait(false);
        var countCommand = CreateCountCommand(username, cancellationToken);
        return new PageModel<UserListModel>
        {
            Items = models.ToArray(),
            Page = pagination.Page,
            PageSize = pagination.PageSize,
            TotalCount = await dbConnection.ExecuteScalarAsync<int>(countCommand).ConfigureAwait(false)
        };
    }

    private static CommandDefinition CreateSearchCommand(
        string username,
        IPagination pagination,
        CancellationToken cancellationToken
    ) =>
        new(
            commandText: """
                         SELECT id, username
                         FROM profiles
                         WHERE username ILIKE (@username)
                         LIMIT @pageSize 
                         OFFSET @page
                         """,
            parameters: new
            {
                username = $"{username}%",
                page = (pagination.Page - 1) * pagination.PageSize,
                pageSize = pagination.PageSize
            },
            cancellationToken: cancellationToken
        );

    private static CommandDefinition CreateCountCommand(string username, CancellationToken cancellationToken) =>
        new(
            commandText: """
                         SELECT COUNT(*) 
                         FROM profiles 
                         WHERE username ILIKE (@username) 
                         """,
            parameters: new
            {
                username = $"{username}%"
            },
            cancellationToken: cancellationToken
        );
}