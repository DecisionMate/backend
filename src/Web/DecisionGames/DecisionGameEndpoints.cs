using System.Diagnostics.CodeAnalysis;
using DecisionMate.Application.DecisionGames.Commands;
using DecisionMate.Application.DecisionGames.Queries;
using DecisionMate.Domain.DecisionGames;
using DecisionMate.Domain.DecisionGames.Models;
using Geneirodan.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DecisionMate.Web.DecisionGames;

public static class DecisionGameEndpoints
{
    // ReSharper disable once UnusedMethodReturnValue.Global
    public static IEndpointRouteBuilder MapDecisionGameEndpoints(
        this IEndpointRouteBuilder endpoints,
        [StringSyntax("Route")] string prefix
    )
    {
        var group = endpoints.MapGroup(prefix)
            .WithTags(nameof(DecisionGame))
            .WithDescription("Endpoints for the decision games.")
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesValidationProblem();

        group.MapPost("/", CreateGame)
            .Produces<DecisionGameModel>(StatusCodes.Status201Created);

        group.MapPatch("/{code}", JoinGame)
            .Produces(StatusCodes.Status200OK);

        var idGroup = group.MapGroup("/{id:guid}")
            .ProducesProblem(StatusCodes.Status404NotFound);

        idGroup.MapGet("/", GetGame)
            .Produces<DecisionGameModel>();

        idGroup.MapPut("/", UpdateGame)
            .Produces<DecisionGameModel>();

        idGroup.MapPost("/code", CreateNewCode)
            .Produces<string>(StatusCodes.Status201Created);

        idGroup.MapPatch("/choices", SetChoices)
            .Produces(StatusCodes.Status200OK);

        idGroup.MapPatch("/play", PlayGame)
            .Produces(StatusCodes.Status200OK);

        idGroup.MapDelete("/", DeleteGame)
            .Produces(StatusCodes.Status200OK);
        
        idGroup.MapDelete("/leave", LeaveGame)
            .Produces(StatusCodes.Status200OK);
        
        return group;
    }

    private static async Task<IResult> DeleteGame(
        [FromRoute] Guid id,
        [FromServices] ISender sender,
        CancellationToken token
    )
    {
        var query = new DeleteGameCommand(id);
        var response = await sender.Send(query, token);
        return response.MapResult();
    }

    private static async Task<IResult> LeaveGame(
        [FromRoute] Guid id,
        [FromServices] ISender sender,
        CancellationToken token
    )
    {
        var query = new LeaveGameCommand(id);
        var response = await sender.Send(query, token);
        return response.MapResult();
    }

    private static async Task<IResult> PlayGame(
        [FromRoute] Guid id,
        [FromServices] ISender sender,
        CancellationToken token
    )
    {
        var query = new PlayGameCommand(id);
        var response = await sender.Send(query, token);
        return response.MapResult();
    }

    private static async Task<IResult> SetChoices(
        [FromRoute] Guid id,
        [FromBody] int[] choices,
        [FromServices] ISender sender,
        CancellationToken token
    )
    {
        var query = new SetPlayerChoicesCommand(id, choices);
        var response = await sender.Send(query, token);
        return response.MapResult();
    }

    private static async Task<IResult> JoinGame(
        [FromRoute] string code,
        [FromServices] ISender sender,
        HttpContext context,
        CancellationToken token
    )
    {
        var command = new JoinGameCommand(code);
        var response = await sender.Send(command, token);
        return response.MapResult();
    }

    private static async Task<IResult> GetGame(
        [FromRoute] Guid id,
        [FromServices] ISender sender,
        CancellationToken token
    )
    {
        var query = new GetGameByIdQuery(id);
        var response = await sender.Send(query, token);
        return response.MapResult();
    }

    private static async Task<IResult> CreateNewCode(
        [FromRoute] Guid id,
        [FromServices] ISender sender,
        CancellationToken token
    )
    {
        var query = new CreateNewCodeCommand(id);
        var response = await sender.Send(query, token);
        return response.MapResult();
    }

    private static async Task<IResult> CreateGame(
        [FromBody] CreateDecisionGameRequest request,
        [FromServices] ISender sender,
        CancellationToken token
    )
    {
        var command = request.MapToCommand();
        var response = await sender.Send(command, token);
        return response.MapResult();
    }

    private static async Task<IResult> UpdateGame(
        [FromRoute] Guid id,
        [FromBody] UpdateDecisionGameRequest request,
        [FromServices] ISender sender,
        CancellationToken token
    )
    {
        var command = request.MapToCommand(id);
        var response = await sender.Send(command, token);
        return response.MapResult();
    }
}