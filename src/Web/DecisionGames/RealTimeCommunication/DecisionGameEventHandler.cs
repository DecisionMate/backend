using DecisionMate.Domain.DecisionGames.Events;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace DecisionMate.Web.DecisionGames.RealTimeCommunication;

internal sealed class DecisionGameEventHandler<T>(IHubContext<DecisionGameHub> hub)
    : INotificationHandler<T> where T : IDecisionGameEvent
{
    public async Task Handle(T notification, CancellationToken cancellationToken) =>
        await hub.Clients.Group(groupName: notification.Id.ToString()).SendAsync(
            method: notification.GetType().Name,
            arg1: notification,
            cancellationToken: cancellationToken
        );
}