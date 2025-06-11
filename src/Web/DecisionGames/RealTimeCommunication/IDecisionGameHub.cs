using JetBrains.Annotations;
using TypedSignalR.Client;

namespace DecisionMate.Web.DecisionGames.RealTimeCommunication;

[Hub, PublicAPI]
public interface IDecisionGameHub
{
    Task Subscribe(Guid gameId);
}