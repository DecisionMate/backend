using DecisionMate.Application.Categories.Providers;
using DecisionMate.Integrations.TheMovieDb;
using Microsoft.Extensions.DependencyInjection;

namespace DecisionMate.Integrations;

public static class DependencyInjection
{
    public static IServiceCollection AddProviders(this IServiceCollection services)
    {
        services.AddOptions<TheMovieDbOptions>()
            .BindConfiguration(nameof(TheMovieDbOptions))
            .ValidateOnStart();
        services.AddHttpClient<ICategoryProvider, MovieDbProvider>();
        return services;
    }
}