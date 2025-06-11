using System.Globalization;
using DecisionMate.Domain.Common.Resources;
using JetBrains.Annotations;

namespace DecisionMate.Domain.DecisionGames.Exceptions;

public sealed class NotEnoughPlayersException() : InvalidOperationException(Message)
{
    public new static string Message => string.Format(
        provider: CultureInfo.CurrentUICulture,
        format: ErrorMessages.NotEnoughPlayers,
        arg0: MinPlayers
    );

    [PublicAPI] public const int MinPlayers = 2;

    public static void EnsurePlayerCount(int count)
    {
        if (count < MinPlayers)
            throw new NotEnoughPlayersException();
    }
}