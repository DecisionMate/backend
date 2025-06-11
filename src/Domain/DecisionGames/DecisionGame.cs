using System.Runtime.CompilerServices;
using DecisionMate.Domain.Common;
using DecisionMate.Domain.Common.Collections;
using DecisionMate.Domain.DecisionGames.Abstractions;
using DecisionMate.Domain.DecisionGames.Events;
using DecisionMate.Domain.DecisionGames.Exceptions;
using DecisionMate.Domain.DecisionGames.ValueObjects;
using Geneirodan.Abstractions.Domain;
using JetBrains.Annotations;

namespace DecisionMate.Domain.DecisionGames;

public sealed class DecisionGame : AggregateRoot
{
    private readonly HashSet<Player> _players = [];
    private JoinCode? _code;
    private List<DecisionResult> _results = [];
    private GameSettings _settings;
    public Guid CategoryId { get; init; }
    public Guid HostId { get; init; }

    public GameSettings Settings
    {
        get => _settings;
        init => _settings = value;
    }

    public JoinCode? Code
    {
        get => _code;
        init => _code = value;
    }

    public IReadOnlyCollection<DecisionResult> Results
    {
        get => _results;
        init => _results = value.ToList();
    }

    public IReadOnlyCollection<Player> Players
    {
        get => _players.ToHashSet();
        init => _players = value.ToHashSet();
    }

    public static (DecisionGame, GameCreatedEvent) Create(Guid categoryId, GameSettings settings)
    {
        var game = new DecisionGame
        {
            Id = Guid.CreateVersion7(),
            CategoryId = categoryId,
            Settings = settings
        };
        var @event = new GameCreatedEvent(game.Id, game.CategoryId, game.Settings);
        game.AddEvent(@event);
        return (game, @event);
    }

    public GameUpdatedEvent Update(GameSettings settings)
    {
        _settings = settings;
        var @event = new GameUpdatedEvent(Id, _settings, _code);
        AddEvent(@event);
        return @event;
    }

    public GameCodeUpdatedEvent UpdateCode(JoinCode? joinCode)
    {
        _code = joinCode;
        var @event = new GameCodeUpdatedEvent(Id, _code);
        AddEvent(@event);
        return @event;
    }

    public PlayerAddedEvent AddPlayer(Player player)
    {
        if (!_players.Add(player))
            throw new PlayerAlreadyParticipatingException(player.Id);
        var @event = new PlayerAddedEvent(Id, player);
        AddEvent(@event);
        return @event;
    }

    public PlayerRemovedEvent RemovePlayer(Guid playerId)
    {
        if (_players.RemoveWhere(player => player.Id == playerId) == 0)
            throw new PlayerNotParticipatingException(playerId);
        var @event = new PlayerRemovedEvent(Id, playerId);
        AddEvent(@event);
        return @event;
    }

    public PlayerChoicesSetEvent SetPlayerChoices(Guid playerId, Player.IChoices choices)
    {
        var player = _players.FirstOrDefault(x => x.Id == playerId);
        if (player is null)
            throw new PlayerNotParticipatingException(playerId);
        player.Choices = choices;
        var @event = new PlayerChoicesSetEvent(Id, playerId, choices);
        AddEvent(@event);
        return @event;
    }

    public GamePlayedEvent Decide(IAlgorithmResolver resolver)
    {
        NotEnoughPlayersException.EnsurePlayerCount(_players.Count);

        var choices = _players.Select(player =>
            player.Choices.Size <= Settings.TopSize
                ? player.Choices
                : throw new InvalidTopSizeException(Settings.TopSize)
        ).ToArray();

        _results = resolver.GetAlgorithm(Settings.Algorithm).Execute(choices).ToList();
        _code = null;
        var @event = new GamePlayedEvent(Id, _results);
        AddEvent(@event);
        return @event;
    }

    public sealed class Player : Entity<Guid>
    {
        public IChoices Choices { get; set; } = [];


        [CollectionBuilder(typeof(ChoicesArray), nameof(ChoicesArray.Create))]
        public interface IChoices : IReadOnlyArray<int>;

        [UsedImplicitly]
        public sealed class ChoicesArray(ReadOnlySpan<int> choices) : ReadOnlyArray<int>(choices), IChoices
        {
            public static IChoices Create(ReadOnlySpan<int> choices)
            {
                return new ChoicesArray(choices);
            }
        }
    }

    public GameDeletedEvent Delete() => new(Id);
}