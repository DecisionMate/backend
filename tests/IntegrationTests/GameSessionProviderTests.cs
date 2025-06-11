using System.Globalization;
using DecisionMate.Application.DecisionGames.Services;
using DecisionMate.Domain.DecisionGames.ValueObjects;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace DecisionMate.IntegrationTests;


[TestSubject(typeof(GameSessionProvider))]
public sealed class GameSessionProviderTests : IntegrationTest
{
    private readonly ITestOutputHelper _output;
    private readonly IGameSessionProvider _gameSessionProvider;

    public GameSessionProviderTests(ApiFactory factory, ITestOutputHelper output) : base(factory)
    {
        _output = output;
        _gameSessionProvider = Scope.ServiceProvider.GetRequiredService<IGameSessionProvider>();
    }

    [Fact]
    public async Task CreateCode_ShouldCreateALotOfCodes()
    {
        HashSet<JoinCode> codes = [];
        for (var i = 0; i < 10_000; i++)
        {
            var code = await _gameSessionProvider.CreateCodeAsync(Guid.NewGuid());
            _output.WriteLine($"{i.ToString(CultureInfo.InvariantCulture)}: {code}");
            codes.Add(code);
        }
        codes.Count.ShouldBe(10_000);
    }
    
    [Fact]
    public async Task GetGameSessionByCode_ShouldReturnId_WhenCodeIsValid()
    {
        var id = Guid.NewGuid();
        var code = await _gameSessionProvider.CreateCodeAsync(id);
        var result = await _gameSessionProvider.GetGameSessionByCodeAsync(code);
        result.ShouldBe(id);
    }

    [Fact] 
    public async Task GetGameSessionByCode_ShouldReturnNull_WhenCodeIsInvalid()
    {
        var id = await _gameSessionProvider.GetGameSessionByCodeAsync(new JoinCode("none"));
        id.ShouldBeNull();
    }
}