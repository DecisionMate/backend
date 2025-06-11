using DecisionMate.Domain.Common.Resources;

namespace DecisionMate.Domain.DecisionGames.Exceptions;

public sealed class PlayerAlreadyParticipatingException(Guid playerId) 
    : InvalidOperationException(string.Format(ErrorMessages.PlayerAlreadyParticipating, playerId));