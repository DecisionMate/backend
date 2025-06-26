using System.Reflection;
using System.Text.Json.Serialization;
using DecisionMate.Application;
using DecisionMate.Domain.DecisionGames.Events;
using DecisionMate.Infrastructure;
using DecisionMate.Integrations;
using DecisionMate.Web.Categories;
using DecisionMate.Web.DecisionGames;
using DecisionMate.Web.DecisionGames.RealTimeCommunication;
using DecisionMate.Web.UserProfiles;
using Geneirodan.Abstractions.Domain;
using Geneirodan.AspNetCore;
using Geneirodan.MediatR;
using Geneirodan.Observability;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using Scalar.AspNetCore;
using TypedSignalR.Client.DevTools;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

builder.AddSerilog();
builder.Services.AddMemoryCache();
builder.Services.AddStackExchangeRedisCache(o => o.Configuration = configuration.GetConnectionString("Redis"));
builder.Services.AddSignalR();
builder.Services
    .ConfigureHttpJsonOptions(o => o.SerializerOptions.Converters.Add(new JsonStringEnumConverter()))
    .AddOpenApi(o =>
    {
        o.OpenApiVersion = OpenApiSpecVersion.OpenApi3_0;
        o.AddDocumentTransformer<JwtBearerSecuritySchemeTransformer>();
    })
    .AddApplicationServices()
    .AddJwtAuth(options =>
    {
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];
                var path = context.HttpContext.Request.Path;
                if (!string.IsNullOrEmpty(accessToken) &&
                    path.StartsWithSegments("/hubs", StringComparison.OrdinalIgnoreCase))
                    context.Token = accessToken;
                return Task.CompletedTask;
            }
        };
    })
    .AddWebLocalization()
    .AddErrorHandling()
    .AddInfrastructure(configuration.GetConnectionString("Postgres"))
    .AddHttpUser()
    .AddProblemDetails()
    .AddSharedOpenTelemetry(configuration)
    .AddMediatRPipeline(Assembly.GetExecutingAssembly())
    .AddProviders()
    .AddCors(options =>
        options.AddDefaultPolicy(x => x
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()
        )
    )
    .AddHealthChecks();

var app = builder.Build();

if (!app.Environment.IsProduction())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
    app.UseSignalRHubSpecification();
    app.UseSignalRHubDevelopmentUI();
}

if (app.Environment.IsDevelopment())
{
    await using var scope = app.Services.CreateAsyncScope();
    await scope.ServiceProvider.GetRequiredService<ApplicationContext>().Database.MigrateAsync();
}

app.UseCors();
app.UseStatusCodePages();

var v1 = app.MapGroup("/api/v1/").WithOpenApi();
v1.MapCategoryEndpoints("/categories");
v1.MapDecisionGameEndpoints("/games");
v1.MapUserProfileEndpoints("/users");

app.MapHub<DecisionGameHub>("/hubs/games");


app.MapHealthChecksWithJsonSupport();

app.MapGet("/test",
    (IPublisher sender, IUser user, Guid id) => sender.Publish(new PlayerChoicesSetEvent(id, user.Id, [1, 2])));

await app.RunAsync();