using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Schema;
using Ardalis.Result;
using DecisionMate.Application.Categories.Mappers;
using DecisionMate.Application.Categories.Providers;
using DecisionMate.Domain.Categories;
using DecisionMate.Domain.Categories.Options;
using DecisionMate.Domain.Categories.Options.ValueObjects;
using DecisionMate.Domain.Common;
using Geneirodan.Abstractions.Repositories;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DecisionMate.Integrations.TheMovieDb;

public sealed partial class MovieDbProvider(
    HttpClient httpClient,
    HybridCache cache,
    IOptions<TheMovieDbOptions> options,
    ILogger<MovieDbProvider> logger) : MovieCategoryProvider
{
    private readonly TheMovieDbOptions _options = options.Value;

    private readonly JsonSerializerOptions _serializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        TypeInfoResolver = JsonSerializerOptions.Default.TypeInfoResolver
    };

    public override async Task<CategoryTemplateModel> GetFullTemplateAsync(CancellationToken cancellationToken)
    {
        var genres =
            await GetGenresAsync(Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName, cancellationToken)
                .ConfigureAwait(false);
        return GetShortTemplate() with
        {
            FiltersJson = _serializerOptions.GetJsonSchemaAsNode(typeof(IMovieDbFilters))["properties"],
            Filters = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase) { { "genres", genres } }
        };
    }

    private async Task<PageModel<Movie>?> SearchMoviesAsync(
        int page,
        IDictionary<string, string> filters,
        CancellationToken cancellationToken = default
    )
    {
        var queryBuilder = new QueryBuilder(filters)
        {
            { "language", Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName }
        };

        var requestUri = $"{_options.MovieEndpointUrl}?{queryBuilder.ToQueryString()}";

        var message = new HttpRequestMessage(HttpMethod.Get, requestUri);
        message.Headers.Add("Authorization", _options.ApiKey);

        var response = await httpClient.SendAsync(message, cancellationToken).ConfigureAwait(false);

        var pagedResult = await response.Content.ReadFromJsonAsync<MovieResponse>(cancellationToken)
            .ConfigureAwait(false);
        if (pagedResult is not null)
            return new PageModel<Movie>
            {
                Items = pagedResult.Results,
                Page = page,
                PageSize = TheMovieDbOptions.PageSize,
                TotalCount = pagedResult.TotalResults
            };

        LogMoviesError();
        return null;
    }

    private ValueTask<IReadOnlyCollection<Genre>> GetGenresAsync(
        string lang,
        CancellationToken cancellationToken = default
    ) => cache.GetOrCreateAsync(
        key: $"TheMovieDb-Genres-{lang}",
        factory: async ct => await GetGenresInnerAsync(lang, ct).ConfigureAwait(false),
        cancellationToken: cancellationToken
    );

    private async ValueTask<IReadOnlyCollection<Genre>> GetGenresInnerAsync(
        string lang,
        CancellationToken cancellationToken = default
    )
    {
        var queryBuilder = new QueryBuilder { { "language", lang } };

        var requestUri = $"{_options.GenreEndpointUrl}?{queryBuilder.ToQueryString()}";

        var message = new HttpRequestMessage(HttpMethod.Get, requestUri);
        message.Headers.Add("Authorization", _options.ApiKey);

        var response = await httpClient.SendAsync(message, cancellationToken).ConfigureAwait(false);
        var pagedResult = await response.Content.ReadFromJsonAsync<GenresResponse>(cancellationToken)
            .ConfigureAwait(false);

        if (pagedResult is not null)
            return pagedResult.Genres;

        LogGenresError();
        return [];
    }

    [LoggerMessage(LogLevel.Error, "Unable to fetch genres")]
    private partial void LogGenresError();

    [LoggerMessage(LogLevel.Error, "Unable to fetch movies")]
    private partial void LogMoviesError();

    public override async Task<Result<CategoryModel>> GetCategoryAsync(
        int count,
        IDictionary<string, string> filters,
        CancellationToken cancellationToken = default
    )
    {
        NormalizeFilters(filters);

        var result = await SearchMoviesAsync(1, filters, cancellationToken).ConfigureAwait(false);

        if (result is null)
            return Result.Unavailable();

        HashSet<Option> set = [];
        while (set.Count < count && set.Count < result.TotalCount)
        {
            var page = Random.Shared.Next(1, result.TotalPages);
            result = await SearchMoviesAsync(page, filters, cancellationToken).ConfigureAwait(false);

            if (result is null)
                return Result.Unavailable();

            var id = 0;
            foreach (var movie in result.Items)
            {
                if (set.Count >= count)
                    break;
                
                var option = new Option
                {
                    Id = ++id,
                    Name = new OptionName(movie.Title),
                    Description = new OptionDescription(movie.Overview),
                    Metadata = movie.MapToMetadata(),
                    ImageUrl = Url.Create(movie.PosterPath)
                };
                set.Add(option);
            }
        }

        return new CategoryModel(
            Id: Guid.CreateVersion7(),
            Name: _options.CategoryName,
            Description: _options.CategoryDescription,
            ImageUrl: _options.CategoryImageUrl,
            Options: set.Map()
        );
    }

    private static void NormalizeFilters(IDictionary<string, string> filters)
    {
        foreach (var (oldKey, newKey) in IMovieDbFilters.Mappings())
            if (filters.TryGetValue(oldKey, out var value))
            {
                filters[newKey] = value;
                filters.Remove(oldKey);
            }
    }
}