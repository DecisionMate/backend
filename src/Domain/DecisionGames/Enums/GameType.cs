using JetBrains.Annotations;

namespace DecisionMate.Domain.DecisionGames.Enums;

[PublicAPI]
public enum GameType : byte
{
    Slides = 0b01,
    Top = 0b10,
    Hybrid = 0b11
}