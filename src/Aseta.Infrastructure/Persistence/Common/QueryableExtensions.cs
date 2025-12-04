using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Aseta.Infrastructure.Persistence.Common;

internal static class QueryableExtensions
{
    public static IQueryable<T> ApplyTracking<T>(this IQueryable<T> query, bool trackChanges) where T : class
    {
        return trackChanges
            ? query
            : query.AsNoTracking();
    }

    public static IQueryable<T> ApplyInclude<T>(this IQueryable<T> query, params Expression<Func<T, object>>[] includeProperties) where T : class
    {
        return includeProperties.Aggregate(query, (current, includeProperty) =>
            current.Include(includeProperty));
    }
}
