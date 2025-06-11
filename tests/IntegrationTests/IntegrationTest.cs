using Microsoft.Extensions.DependencyInjection;

namespace DecisionMate.IntegrationTests;

public abstract class IntegrationTest(ApiFactory factory) : IClassFixture<ApiFactory>, IDisposable
{
    protected readonly IServiceScope Scope = factory.Services.CreateScope();

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        Scope.Dispose();
    }
}