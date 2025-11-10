using System.Linq.Expressions;
using Aseta.Domain.Abstractions.Persistence;
using Aseta.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Aseta.Infrastructure.Repository;

internal class Repository<T>(AppDbContext context) : IRepository<T> where T : class
{
    protected readonly AppDbContext _context = context;
    protected readonly DbSet<T> _dbSet = context.Set<T>();

    public async Task<IEnumerable<T>> GetAllAsync(
        CancellationToken cancellationToken = default,
        params Expression<Func<T, object>>[] includeProperties)
    {
        IQueryable<T> query = _dbSet;
        query = ApplyIncludes(query, includeProperties);
        return await query.ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<T>> FindAsync(
        Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default,
        params Expression<Func<T, object>>[] includeProperties)
    {
        IQueryable<T> query = _dbSet.Where(predicate);
        query = ApplyIncludes(query, includeProperties);
        return await query.ToListAsync(cancellationToken);
    }

    public async Task<T?> FirstOrDefaultAsync(
        Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default,
        params Expression<Func<T, object>>[] includeProperties)
    {
        IQueryable<T> query = _dbSet;
        query = ApplyIncludes(query, includeProperties);
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

    private static IQueryable<T> ApplyIncludes(
        IQueryable<T> query,
        params Expression<Func<T, object>>[] includeProperties)
    {
        return includeProperties.Aggregate(query, (current, includeProperty) =>
            current.Include(includeProperty));
    }

    public async Task<bool> DeleteAsync(
        Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await _dbSet.Where(predicate).ExecuteDeleteAsync(cancellationToken) > 0;
    }

    public Task<int> CountAsync(
        Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return _dbSet.CountAsync(predicate, cancellationToken);
    }

    public async Task<IEnumerable<T>> GetPagedAsync(
        int pageNumber,
        int pageSize,
        Expression<Func<T, bool>> predicate,
        Expression<Func<T, object>> orderBy,
        bool descending = true,
        CancellationToken cancellationToken = default)
    {
        IQueryable<T> query = _dbSet.Where(predicate);

        IOrderedQueryable<T> orderedQuery = descending
            ? query.OrderByDescending(orderBy)
            : query.OrderBy(orderBy);

        return await orderedQuery
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }
}