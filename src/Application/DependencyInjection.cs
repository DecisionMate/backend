using System.Reflection;
using DecisionMate.Application.DecisionGames.Services;
using DecisionMate.Application.DecisionGames.Services.Algorithms;
using DecisionMate.Domain.DecisionGames.Abstractions;
using FluentValidation;
using Geneirodan.MediatR;
using Geneirodan.MediatR.Options;
using Microsoft.Extensions.DependencyInjection;

namespace DecisionMate.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();
        services.AddHybridCache();
        return services
            .AddTransient<IAlgorithmResolver, AlgorithmResolver>()
            .AddScoped<IGameSessionProvider, GameSessionProvider>()
            .AddMediatRPipeline(new MediatRPipelineOptions
            {
                UseValidation = true
            }, assembly)
            .AddValidatorsFromAssembly(assembly, includeInternalTypes: true);
    }
}