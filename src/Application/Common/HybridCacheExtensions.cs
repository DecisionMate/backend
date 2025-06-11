using Microsoft.Extensions.Caching.Hybrid;

namespace DecisionMate.Application.Common;

public static class HybridCacheExtensions
{
    public static ValueTask<T?> GetAsync<T>(this HybridCache cache, string code)
        where T : class =>
        cache.GetOrCreateAsync<T?>(code, _ => ValueTask.FromResult<T?>(null));
}