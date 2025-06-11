using DecisionMate.Domain.Common.Resources;

namespace DecisionMate.Domain.DecisionGames.Exceptions;

public sealed class PlayerNotParticipatingException(Guid playerId) 
    : InvalidOperationException(string.Format(ErrorMessages.PlayerNotParticipating, playerId));