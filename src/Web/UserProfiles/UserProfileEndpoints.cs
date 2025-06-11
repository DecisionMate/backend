using System.Diagnostics.CodeAnalysis;
using DecisionMate.Application.UserProfiles.Commands;
using DecisionMate.Application.UserProfiles.Queries;
using DecisionMate.Domain.Users;
using Geneirodan.Abstractions.Repositories;
using Geneirodan.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DecisionMate.Web.UserProfiles;

public static class UserProfileEndpoints
{
    // ReSharper disable once UnusedMethodReturnValue.Global
    public static IEndpointRouteBuilder MapUserProfileEndpoints(
        this IEndpointRouteBuilder endpoints,
        [StringSyntax("Route")] string prefix
    )
    {
        var group = endpoints.MapGroup(prefix)
            .WithTags(nameof(UserProfile))
            .WithDescription("Endpoints for the user profiles")
            .ProducesValidationProblem();

        group.MapGet("/", GetProfiles)
            .Produces<PageModel<UserListModel>>();

        group.MapPost("/", CreateProfile)
            .Produces<Guid>(StatusCodes.Status201Created);

        var idGroup = group.MapGroup("/{id:guid}")
            .ProducesProblem(StatusCodes.Status404NotFound);

        idGroup.MapDelete("/", DeleteProfile)
            .Produces(StatusCodes.Status200OK);

        return group;
    }

    private static async Task<IResult> GetProfiles(
        [AsParameters] UserProfilesGridifyQuery parameters,
        [FromQuery] string username,
        [FromServices] ISender sender,
        CancellationToken token
    )
    {
        var query = new GetUserProfilesQuery(username, parameters);
        var result = await sender.Send(query, token);
        return result.MapResult();
    }

    private static async Task<IResult> CreateProfile(
        [FromBody] AddUserProfileRequest request,
        [FromServices] ISender sender,
        CancellationToken token
    )
    {
        var command = request.MapToCommand();
        var response = await sender.Send(command, token);
        return response.MapResult();
    }

    private static async Task<IResult> DeleteProfile(
        [FromRoute] Guid id,
        [FromServices] ISender sender,
        CancellationToken token
    )
    {
        var command = new DeleteUserProfileCommand(id);
        var response = await sender.Send(command, token);
        return response.MapResult();
    }
}