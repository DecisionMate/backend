using DecisionMate.Application.Common;
using DecisionMate.Infrastructure.Common;
using FluentValidation;
using Geneirodan.Abstractions.Mapping;
using Geneirodan.Abstractions.Repositories;
using Geneirodan.EntityFrameworkCore;
using Gridify;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DecisionMate.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string? connectionString) =>
        services
            .Scan(scan => scan
                .FromAssemblyOf<ApplicationContext>()
                .AddClasses(c => c
                    .AssignableToAny(
                        typeof(IEntityMapper<,>),
                        typeof(IRepository<,>),
                        typeof(IGridifyMapper<>)
                    )
                )
                .AsImplementedInterfaces()
                .WithScopedLifetime()
            )
            .AddScoped(typeof(IGridifyValidator<>), typeof(GridifyValidator<>))
            .AddValidatorsFromAssemblyContaining<ApplicationContext>()
            .AddDbContext<DbContext, ApplicationContext>(x => x.UseNpgsql(connectionString).UseSnakeCaseNamingConvention())
            .AddScoped<IUnitOfWork, UnitOfWork>();
}