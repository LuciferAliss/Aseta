using System.Linq.Expressions;
using Aseta.Domain.Abstractions.Primitives.Entities;

namespace Aseta.Domain.Abstractions.Persistence;

public interface IRepository<T> where T : IEntity
{
    Task AddAsync(T entity, CancellationToken cancellationToken = default);
    Task<int> BulkRemoveAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
    void Remove(T entity);
    Task<T?> GetByIdAsync(Guid id, bool trackChanges = default, CancellationToken cancellationToken = default, params Expression<Func<T, object>>[] includeProperties);
    Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<T>> GetAllAsync(
        Expression<Func<T, bool>> predicate, 
        bool trackChanges = default, 
        CancellationToken cancellationToken = default, 
        params Expression<Func<T, object>>[] includeProperties);
    Task<T?> FirstOrDefaultAsync(
        Expression<Func<T, bool>> predicate, 
        bool trackChanges = default, 
        CancellationToken cancellationToken = default, 
        params Expression<Func<T, object>>[] includeProperties);
}