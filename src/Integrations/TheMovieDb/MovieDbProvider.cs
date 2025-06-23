using System.Globalization;
using System.Net.Http.Json;
using System.Web;
using Ardalis.Result;
using DecisionMate.Application.Categories.Mappers;
using DecisionMate.Domain.Categories;
using DecisionMate.Domain.Categories.Options;
using DecisionMate.Domain.Categories.Options.ValueObjects;
using DecisionMate.Domain.Common;
using Geneirodan.Abstractions.Repositories;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DecisionMate.Integrations.TheMovieDb;

public partial class MovieDbProvider(
    HttpClient httpClient,
    HybridCache cache,
    IOptions<TheMovieDbOptions> options,
    ILogger<MovieDbProvider> logger) : IMoviesCategoryProvider
{
    private readonly TheMovieDbOptions _options = options.Value;

    private async Task<PageModel<Movie>?> SearchMoviesAsync(
        int page,
        string? withGenres = null,
        string? withoutGenres = null,
        string? withCountries = null,
        string? withoutCountries = null,
        int? startYear = null,
        int? endYear = null,
        CancellationToken cancellationToken = default
    )
    {
        var queryString = HttpUtility.ParseQueryString(string.Empty);
        if (withGenres is not null)
            queryString.Add(
                name: "with_genres",
                value: withGenres
            );

        if (withoutGenres is not null)
            queryString.Add(
                name: "with_genres",
                value: withoutGenres
            );

        if (withCountries is not null)
            queryString.Add(
                name: "with_countries",
                value: withCountries
            );

        if (withoutCountries is not null)
            queryString.Add(
                name: "without_countries",
                value: withoutCountries
            );

        if (startYear is not null)
            queryString.Add(
                name: "release_date.gte",
                value: $"{startYear.Value.ToString(CultureInfo.InvariantCulture)}-01-01"
            );

        if (endYear is not null)
            queryString.Add(
                name: "release_date.lte",
                value: $"{endYear.Value.ToString(CultureInfo.InvariantCulture)}-01-01"
            );

        queryString.Add("language", Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName);
        var message = new HttpRequestMessage(HttpMethod.Get, $"{_options.MovieEndpointUrl}?{queryString}");
        message.Headers.Add("Authorization", _options.ApiKey);
        var response = await httpClient
            .SendAsync(message, cancellationToken: cancellationToken)
            .ConfigureAwait(false);
        var pagedResult = await response.Content.ReadFromJsonAsync<MovieResponse>(cancellationToken: cancellationToken)
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

    public ValueTask<IReadOnlyCollection<Genre>?> GetGenresAsync(
        string lang = "en",
        CancellationToken cancellationToken = default
    ) => cache.GetOrCreateAsync(
        key: $"TheMovieDb-Genres-{lang}",
        factory: async ct => await GetGenresInnerAsync(lang, ct).ConfigureAwait(false),
        cancellationToken: cancellationToken
    );

    private async ValueTask<IReadOnlyCollection<Genre>?> GetGenresInnerAsync(
        string lang = "en",
        CancellationToken cancellationToken = default
    )
    {
        var queryString = HttpUtility.ParseQueryString(_options.GenreEndpointUrl);
        queryString.Add("language", lang);
        var pagedResult = await httpClient
            .GetFromJsonAsync<GenresResponse>(queryString.ToString(), cancellationToken: cancellationToken)
            .ConfigureAwait(false);
        if (pagedResult is not null)
            return pagedResult.Genres;

        LogGenresError();
        return null;
    }

    [LoggerMessage(LogLevel.Error, "Unable to fetch genres")]
    private partial void LogGenresError();

    [LoggerMessage(LogLevel.Error, "Unable to fetch movies")]
    private partial void LogMoviesError();

    public async Task<Result<CategoryModel>> GetCategoryAsync(
        int count,
        string? withGenres = null,
        string? withoutGenres = null,
        string? withCountries = null,
        string? withoutCountries = null,
        int? startYear = null,
        int? endYear = null,
        CancellationToken cancellationToken = default
    )
    {
        var result = await SearchMoviesAsync(
            1,
            withGenres,
            withoutGenres,
            withCountries,
            withoutCountries,
            startYear,
            endYear,
            cancellationToken
        ).ConfigureAwait(false);

        if (result is null)
            return Result.Unavailable();

        HashSet<Option> set = [];
        do
        {
            result = await SearchMoviesAsync(
                page: Random.Shared.Next(1, result.TotalPages),
                withGenres: withGenres,
                withoutGenres: withoutGenres,
                withCountries: withCountries,
                withoutCountries: withoutCountries,
                startYear: startYear,
                endYear: endYear,
                cancellationToken: cancellationToken
            ).ConfigureAwait(false);

            if (result is null)
                return Result.Unavailable();

            var id = 0;
            foreach (var movie in result.Items)
            {
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
        } while (set.Count < count);

        return new CategoryModel(
            Id: Guid.CreateVersion7(),
            Name: _options.CategoryName,
            Description: _options.CategoryDescription,
            ImageUrl: _options.CategoryImageUrl,
            Options: set.Map()
        );
    }
}