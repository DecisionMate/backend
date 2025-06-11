using TypedSignalR.Client;

namespace DecisionMate.Web.DecisionGames.RealTimeCommunication;

[Receiver]
public interface IDecisionGameReceiver
{
    Task Status(string user, string message);
    Task UserConnected(Guid user);
    Task Error(string errorMessage);
}