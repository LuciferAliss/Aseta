using System.Linq.Expressions;
using Aseta.Domain.Abstractions.Persistence;
using Aseta.Domain.Abstractions.Primitives.Entities;
using Aseta.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Aseta.Infrastructure.Persistence.Common;

internal class Repository<T>(AppDbContext context) : IRepository<T> where T : class, IEntity
{
    protected readonly AppDbContext _context = context;
    protected readonly DbSet<T> _dbSet = context.Set<T>();

    public async Task<IReadOnlyCollection<T>> GetAllAsync(
        bool trackChanges = default,
        CancellationToken cancellationToken = default,
        params Expression<Func<T, object>>[] includeProperties)
    {
        var query = _dbSet;
        ApplyInclude(query, includeProperties);
        ApplyTracking(query, trackChanges);
        
        return await query.ToListAsync(cancellationToken);
    }

    public virtual async Task<T?> GetByIdAsync<TId>(
        TId id,
        bool trackChanges = default,
        CancellationToken cancellationToken = default,
        params Expression<Func<T, object>>[] includeProperties)
    {
        var query = _dbSet;
        ApplyTracking(query, trackChanges);
        ApplyInclude(query, includeProperties);

        return await query.FindAsync([id], cancellationToken: cancellationToken);
    }

    public async Task<T?> FirstOrDefaultAsync(
        Expression<Func<T, bool>> predicate,
        bool trackChanges = default,
        CancellationToken cancellationToken = default,
        params Expression<Func<T, object>>[] includeProperties)
    {
        var query = _dbSet;
        ApplyInclude(query, includeProperties);
        ApplyTracking(query, trackChanges);

        return await query.FirstOrDefaultAsync(predicate, cancellationToken);
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

    public async Task<int> RemoveAsync(
        Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await _dbSet.Where(predicate).ExecuteDeleteAsync(cancellationToken);
    }

    protected static IQueryable<T> ApplyTracking(IQueryable<T> query, bool trackChanges)
    {
        return trackChanges 
            ? query 
            : query.AsNoTracking();
    }

    protected static IQueryable<T> ApplyInclude(IQueryable<T> query, params Expression<Func<T, object>>[] includeProperties)
    {
        return includeProperties.Aggregate(query, (current, includeProperty) =>
            current.Include(includeProperty));
    }
}