using System.Net.Http.Json;
using System.Web;
using Ardalis.Result;
using Geneirodan.Abstractions.Repositories;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DecisionMate.Integrations.TheMovieDb;

public partial class MovieDbProvider(
    HttpClient httpClient,
    HybridCache cache,
    IOptions<TheMovieDbOptions> options,
    ILogger<MovieDbProvider> logger)
{
    private readonly TheMovieDbOptions _options = options.Value;

    public async Task<Result<PageModel<Movie>>> SearchMoviesAsync(int page, string[]? genres = null, int? year = null,
        CancellationToken cancellationToken = default)
    {
        var queryString = HttpUtility.ParseQueryString(_options.MovieEndpointUrl);
        if (genres is not null)
        {
            var genresCollection = await GetOrCreateGenresInCacheAsync(cancellationToken)
                .ConfigureAwait(false);
            if (genresCollection is null)
            {
                LogGenresError();
                return Result.Unavailable();
            }

            foreach (var genre in genres)
                queryString.Add("with_genres", genre);
        }
        

        queryString.Add("language", Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName);
        var pagedResult = await httpClient
            .GetFromJsonAsync<MovieResponse>(queryString.ToString(), cancellationToken: cancellationToken)
            .ConfigureAwait(false);
        if (pagedResult is not null)
            return new PageModel<Movie>
            {
                Items = pagedResult.Results,
                Page = page,
                PageSize = 20,
                TotalCount = pagedResult.TotalResults
            };
        
        LogMoviesError();
        return Result.Unavailable();
    }

    private ValueTask<Genre[]?> GetOrCreateGenresInCacheAsync(CancellationToken cancellationToken) =>
        cache.GetOrCreateAsync(
            key: "TheMovieDb-Genres",
            factory: async ct => await GetGenresAsync("en", ct).ConfigureAwait(false),
            cancellationToken: cancellationToken
        );

    private async ValueTask<Genre[]?> GetGenresAsync(string? lang = "en", CancellationToken cancellationToken = default)
    {
        var queryString = HttpUtility.ParseQueryString(_options.MovieEndpointUrl);
        queryString.Add("language", lang);
        var pagedResult = await httpClient
            .GetFromJsonAsync<GenresResponse>(queryString.ToString(), cancellationToken: cancellationToken)
            .ConfigureAwait(false);
        return pagedResult?.Genres;
    }

    public async ValueTask<Result<Dictionary<string, string>>> GetGenresAsync(CancellationToken cancellationToken = default)
    {
        var queryString = HttpUtility.ParseQueryString(_options.MovieEndpointUrl);
        queryString.Add("language", Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName);
        var pagedResult = await httpClient
            .GetFromJsonAsync<GenresResponse>(queryString.ToString(), cancellationToken: cancellationToken)
            .ConfigureAwait(false);
        var cachedGenres = await GetOrCreateGenresInCacheAsync(cancellationToken).ConfigureAwait(false);
        
        if (cachedGenres is not null && pagedResult is not null)
            return pagedResult.Genres
                .Join(cachedGenres, x => x.Id, x => x.Id, (x, y) => (x.Name, y.Name))
                .ToDictionary(x => x.Item1, x => x.Item2);
        
        LogGenresError();
        return Result.Unavailable();

    }

    [LoggerMessage(LogLevel.Error, "Unable to fetch genres")]
    private partial void LogGenresError();
    
    [LoggerMessage(LogLevel.Error, "Unable to fetch movies")]
    private partial void LogMoviesError();
}