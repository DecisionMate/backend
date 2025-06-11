using DecisionMate.Domain.DecisionGames;
using DecisionMate.Domain.DecisionGames.Abstractions;
using DecisionMate.Domain.DecisionGames.Enums;
using DecisionMate.Domain.DecisionGames.ValueObjects;
using JetBrains.Annotations;
using Moq;
using Shouldly;

namespace DecisionMate.Domain.Tests.DecisionGames;

[TestSubject(typeof(DecisionGame))]
public sealed class DecisionGameTests
{
    private readonly Mock<IAlgorithmResolver> _resolverMock = new();
    private readonly Mock<IDecisionAlgorithm> _algorithmMock = new();

    public DecisionGameTests() =>
        _resolverMock
            .Setup(r => r.GetAlgorithm(It.IsAny<Algorithm>()))
            .Returns(_algorithmMock.Object);

    [Fact]
    public void AddPlayer_AddsPlayerSuccessfully()
    {
        var game = CreateDefaultDecisionGame();
        var player = new DecisionGame.Player { Id = Guid.NewGuid() };

        game.AddPlayer(player);

        game.Players.ShouldContain(player);
    }

    [Fact]
    public void AddPlayer_ThrowsWhenPlayerAlreadyAdded()
    {
        var game = CreateDefaultDecisionGame();
        var player = new DecisionGame.Player { Id = Guid.NewGuid() };

        game.AddPlayer(player);

        var exception = Should.Throw<InvalidOperationException>(() => game.AddPlayer(player));
        exception.Message.ShouldBe($"Player {player.Id} was already added");
    }

    [Fact]
    public void SetPlayerChoices_SetsChoicesSuccessfully()
    {
        var game = CreateDefaultDecisionGame();
        var player = new DecisionGame.Player { Id = Guid.NewGuid() };
        game.AddPlayer(player);

        DecisionGame.Player.IChoices choices = [1, 2, 3];
        game.SetPlayerChoices(player.Id, choices);
        player.Choices.ShouldBe(choices);
    }

    [Fact]
    public void SetPlayerChoices_ThrowsWhenPlayerNotFound()
    {
        var game = CreateDefaultDecisionGame();
        var playerId = Guid.NewGuid();

        var exception = Should.Throw<InvalidOperationException>(() => game.SetPlayerChoices(playerId, [1, 2, 3]));
        exception.Message.ShouldBe($"Player {playerId} is not participant of the game");
    }

    [Fact]
    public void RemovePlayer_RemovesPlayerSuccessfully()
    {
        var game = CreateDefaultDecisionGame();
        var player = new DecisionGame.Player { Id = Guid.NewGuid() };
        game.AddPlayer(player);

        game.RemovePlayer(player.Id);
        game.Players.ShouldNotContain(player);
    }

    [Fact]
    public void Decide_ExecutesAlgorithmSuccessfully()
    {
        var game = CreateDefaultDecisionGame();
        var player1 = new DecisionGame.Player { Id = Guid.NewGuid() };
        var player2 = new DecisionGame.Player { Id = Guid.NewGuid() };
        game.AddPlayer(player1);
        game.SetPlayerChoices(player1.Id, [1, 2]);
        game.AddPlayer(player2);
        game.SetPlayerChoices(player2.Id, [3, 4]);

        _algorithmMock.Setup(a => a.Execute(It.IsAny<DecisionGame.Player.IChoices[]>()))
            .Returns([new DecisionResult(2, 20), new DecisionResult(1, 10)]);

        game.Decide(_resolverMock.Object);

        game.Results.ShouldBe([(2, 20), (1, 10)]);
    }

    [Fact]
    public void Decide_ThrowsWhenNoPlayers()
    {
        var game = CreateDefaultDecisionGame();

        var exception = Should.Throw<InvalidOperationException>(() => game.Decide(_resolverMock.Object));
        exception.Message.ShouldBe("At least 2 players are required");
    }

    private static DecisionGame CreateDefaultDecisionGame() =>
        new()
        {
            Id = Guid.NewGuid(),
            CategoryId = Guid.Empty,
            Settings = new GameSettings
            {
                Type = GameType.Hybrid,
                Algorithm = Algorithm.Condorcet,
                NumberOfWinners = 2,
                TopSize = 5
            }
        };
}