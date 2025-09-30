using System.Linq.Expressions;

namespace Aseta.Domain.Abstractions.Repository;

public interface IRepository<T> where T : class
{
    IQueryable<T> GetQueryable();

    Task AddAsync(T entity, CancellationToken cancellationToken = default);

    Task DeleteAsync(T entity);

    Task<IEnumerable<T>> FindAsync(
        Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}