using DecisionMate.Web;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Testcontainers.PostgreSql;
using Testcontainers.Redis;

namespace DecisionMate.IntegrationTests;

[UsedImplicitly]
public sealed class ApiFactory : WebApplicationFactory<IApiMarker>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgres = new PostgreSqlBuilder().Build();
    private readonly RedisContainer _redis = new RedisBuilder().Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        
        builder.UseSetting("ConnectionStrings:Postgres", _postgres.GetConnectionString());
        builder.UseSetting("ConnectionStrings:Redis", _redis.GetConnectionString());
        builder.ConfigureServices(_ => { });
        base.ConfigureWebHost(builder);
    }

    public async ValueTask InitializeAsync()
    {
        await _redis.StartAsync(TestContext.Current.CancellationToken);
        await _postgres.StartAsync(TestContext.Current.CancellationToken);
    }

    public override async ValueTask DisposeAsync()
    {
        await _redis.StopAsync(TestContext.Current.CancellationToken);
        await _postgres.StopAsync(TestContext.Current.CancellationToken);
    }
}