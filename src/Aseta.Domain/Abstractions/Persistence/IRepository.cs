using System.Linq.Expressions;

namespace Aseta.Domain.Abstractions.Persistence;

public interface IRepository<T> where T : class
{
    Task AddAsync(T entity, CancellationToken cancellationToken = default);

    Task DeleteAsync(T entity);

    Task<int> CountAsync(
        Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default);

    Task BulkDeleteAsync(
        Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<T>> FindAsync(
        Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default,
        params Expression<Func<T, object>>[] includeProperties);

    Task<IEnumerable<T>> GetAllAsync(
        CancellationToken cancellationToken = default,
        params Expression<Func<T, object>>[] includeProperties);

    Task<T?> FirstOrDefaultAsync(
        Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default,
        params Expression<Func<T, object>>[] includeProperties);

    Task<IEnumerable<T>> GetPagedAsync(
        int pageNumber,
        int pageSize,
        Expression<Func<T, bool>> predicate,
        Expression<Func<T, object>> orderBy,
        bool descending = true,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(
        Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default);
}