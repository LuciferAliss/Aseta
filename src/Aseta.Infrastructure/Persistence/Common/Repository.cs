using System.Linq.Expressions;
using Aseta.Domain.Abstractions.Persistence;
using Aseta.Domain.Abstractions.Primitives.Entities;
using Aseta.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Aseta.Infrastructure.Persistence.Common;

public class Repository<T>(AppDbContext context) : IRepository<T> where T : class, IEntity
{
    protected readonly AppDbContext _context = context;
    protected readonly DbSet<T> _dbSet = context.Set<T>();

    public async Task<IReadOnlyCollection<T>> GetAllAsync(
        Expression<Func<T, bool>> predicate,
        bool trackChanges = default,
        CancellationToken cancellationToken = default,
        params Expression<Func<T, object>>[] includeProperties)
    {
        return await _dbSet.ApplyInclude(includeProperties)
            .ApplyTracking(trackChanges)
            .Where(predicate)
            .ToListAsync(cancellationToken);
    }

    public virtual async Task<T?> GetByIdAsync(
        Guid id,
        bool trackChanges = default,
        CancellationToken cancellationToken = default,
        params Expression<Func<T, object>>[] includeProperties)
    {
        return await _dbSet.ApplyInclude(includeProperties)
            .ApplyTracking(trackChanges)
            .FirstOrDefaultAsync(i => i.Id == id, cancellationToken: cancellationToken);
    }

    public async Task<T?> FirstOrDefaultAsync(
        Expression<Func<T, bool>> predicate,
        bool trackChanges = default,
        CancellationToken cancellationToken = default,
        params Expression<Func<T, object>>[] includeProperties)
    {
        return await _dbSet.ApplyInclude(includeProperties)
            .ApplyTracking(trackChanges)
            .FirstOrDefaultAsync(predicate, cancellationToken);
    }

    public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddAsync(entity, cancellationToken);
    }

    public async Task<bool> ExistsAsync(
        Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await _dbSet.AnyAsync(predicate, cancellationToken);
    }

    public async Task<int> BulkRemoveAsync(
        Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await _dbSet.Where(predicate).ExecuteDeleteAsync(cancellationToken);
    }

    public void Remove(T entity)
    {
        _dbSet.Remove(entity);
    }
}