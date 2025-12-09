using System.Text.Json;
using Aseta.Application.Abstractions.Services;
using Microsoft.Extensions.Caching.Distributed;

namespace Aseta.Infrastructure.Caches;

internal sealed class CacheService(
    IDistributedCache distributedCache) : ICacheService
{
    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
    {
        string? value = await distributedCache.GetStringAsync(key, cancellationToken);
        if (value is null)
        {
            return default;
        }

        return JsonSerializer.Deserialize<T?>(value);
    }

    public async Task<T?> GetAsync<T>(string key, Func<Task<T?>> factory, CancellationToken cancellationToken = default)
    {
        T? value = await GetAsync<T>(key, cancellationToken);
        if (value is not null)
        {
            return value;
        }

        value = await factory();
        await SetAsync(key, value, cancellationToken);

        return value;
    }

    public async Task SetAsync<T>(string key, T value, CancellationToken cancellationToken = default)
    {
        await distributedCache.SetStringAsync(key, JsonSerializer.Serialize(value), cancellationToken);
    }

    public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        await distributedCache.RemoveAsync(key, cancellationToken);
    }
}
