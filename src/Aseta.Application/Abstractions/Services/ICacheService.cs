using Aseta.Domain.Abstractions.Primitives;

namespace Aseta.Application.Abstractions.Services;

public interface ICacheService
{
    Task<Result<T>> GetAsync<T>(string key, CancellationToken cancellationToken = default) where T : class;
    Task<Result<T>> GetAsync<T>(string key, Func<T, bool> func, CancellationToken cancellationToken = default) where T : class;
    Task<Result> SetAsync<T>(string key, T value, CancellationToken cancellationToken = default) where T : class;
    Task<Result> DeleteAsync(string key, CancellationToken cancellationToken = default);
    Task<Result> DeleteByPrefixAsync(string prefixKey, CancellationToken cancellationToken = default);
}
