using System.Globalization;
using DecisionMate.Domain.Common.Resources;

namespace DecisionMate.Domain.DecisionGames.Exceptions;

public sealed class InvalidTopSizeException(int optionsCount)
    : InvalidOperationException(CreateMessage(optionsCount))
{
    private static string CreateMessage(int optionsCount) =>
        string.Format(
            provider: CultureInfo.CurrentUICulture,
            format: ErrorMessages.InvalidTopSize,
            arg0: optionsCount
        );
}