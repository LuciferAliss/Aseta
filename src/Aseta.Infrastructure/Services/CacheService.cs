using Aseta.Application.Abstractions.Services;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Aseta.Infrastructure.Services;

internal sealed class CacheService(
    IDistributedCache distributedCache) : ICacheService
{
    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
    {
        string? value = await distributedCache.GetStringAsync(key, cancellationToken);
        if (value is null) return default;

        return JsonConvert.DeserializeObject<T>(value);
    }

    public async Task<T?> GetAsync<T>(string key, Func<Task<T?>> factory, CancellationToken cancellationToken = default)
    {
        var value = await GetAsync<T>(key, cancellationToken);
        if (value is not null) return value;

        value = await factory();
        await SetAsync(key, value, cancellationToken);

        return value;
    }

    public async Task SetAsync<T>(string key, T value, CancellationToken cancellationToken = default)
    {
        await distributedCache.SetStringAsync(key, JsonConvert.SerializeObject(value), cancellationToken);
    }

    public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        await distributedCache.RemoveAsync(key, cancellationToken);
    }
}
