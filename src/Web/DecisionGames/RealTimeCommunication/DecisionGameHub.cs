using DecisionMate.Application.DecisionGames;
using Geneirodan.Abstractions.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace DecisionMate.Web.DecisionGames.RealTimeCommunication;

[Authorize]
internal sealed class DecisionGameHub(IDecisionGameRepository gameRepository, IUser user) : Hub<IDecisionGameReceiver>, IDecisionGameHub
{
    public async Task Subscribe(Guid gameId)
    {
        if (user.Id is null)
        {
            await Clients.Caller.Error("Unauthorized");
            return;
        }

        var game = await gameRepository.FindAsync(gameId, Context.ConnectionAborted);
        if (game is null)
        {
            await Clients.Caller.Error("Game not found");
            return;
        }

        if (game.Players.All(x => x.Id != user.Id))
        {
            await Clients.Caller.Error("Player not found");
            return;
        }

        await Groups.AddToGroupAsync(Context.ConnectionId, gameId.ToString(), Context.ConnectionAborted);

        await Clients.Group(gameId.ToString()).UserConnected(user.Id!.Value);
    }
}