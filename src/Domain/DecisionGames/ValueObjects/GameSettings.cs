using System.Runtime.InteropServices;
using DecisionMate.Domain.DecisionGames.Enums;

namespace DecisionMate.Domain.DecisionGames.ValueObjects;

[StructLayout(LayoutKind.Auto)]
public readonly record struct GameSettings
{
    public GameType Type { get; init; }
    public Algorithm Algorithm { get; init; }
    public int NumberOfWinners { get; init; }
    public int TopSize { get; init; }
    
    public const int MaxNumberOfWinners = 10;
    public const int MinNumberOfWinners = 1;
    
    public const int MaxTopSize = 10;
    public const int MinTopSize = 1;
}