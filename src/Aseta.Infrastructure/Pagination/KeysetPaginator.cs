using Aseta.Domain.Abstractions.Primitives;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Text;
using System.Text.Json;

namespace Aseta.Infrastructure.Pagination;

public class KeysetPaginator<T>(IQueryable<T> query) where T : class, IEntity
{
    private readonly Dictionary<string, (LambdaExpression keySelector, Type keyType)> _sortFields = [];

    public KeysetPaginator<T> AddSortableField<TKey>(string key, Expression<Func<T, TKey>> keySelector)
    {
        _sortFields.Add(key, (keySelector, typeof(TKey)));
        return this;
    }

    public async Task<(ICollection<T> items, string? nextCursor, bool hasNextPage)> PaginateAsync(
        string sortBy,
        string sortOrder,
        int pageSize,
        string? cursor,
        CancellationToken cancellationToken = default)
    {
        if (!_sortFields.TryGetValue(sortBy, out var sortInfo))
        {
            throw new ArgumentException($"Sort key '{sortBy}' is not registered.", nameof(sortBy));
        }

        bool isDescending = sortOrder.Equals("desc", StringComparison.OrdinalIgnoreCase);

        var orderedQuery = ApplyOrderBy(sortInfo.keySelector, isDescending);

        if (!string.IsNullOrWhiteSpace(cursor))
        {
            orderedQuery = ApplyCursorWhere(orderedQuery, cursor, sortInfo.keySelector, sortInfo.keyType, isDescending);
        }

        var items = await orderedQuery.Take(pageSize + 1).ToListAsync(cancellationToken);

        bool hasNextPage = items.Count > pageSize;
        var pageItems = items.Take(pageSize).ToList();
        string? nextCursor = null;

        if (hasNextPage && pageItems.Count != 0)
        {
            var lastItem = pageItems.Last();
            var sortValue = sortInfo.keySelector.Compile().DynamicInvoke(lastItem);
            nextCursor = EncodeCursor(sortValue, lastItem.Id);
        }

        return (pageItems, nextCursor, hasNextPage);
    }

    private IOrderedQueryable<T> ApplyOrderBy(LambdaExpression keySelector, bool descending)
    {
        var methodName = descending ? nameof(Queryable.OrderByDescending) : nameof(Queryable.OrderBy);

        var method = typeof(Queryable).GetMethods()
            .First(m => m.Name == methodName && m.GetParameters().Length == 2)
            .MakeGenericMethod(typeof(T), keySelector.Body.Type);

        var sortedQuery = (IOrderedQueryable<T>)method.Invoke(null, [query, keySelector])!;

        return sortedQuery.ThenBy(e => e.Id);
    }

    private IOrderedQueryable<T> ApplyCursorWhere(IOrderedQueryable<T> query, string cursor, LambdaExpression keySelector, Type keyType, bool descending)
    {
        var (sortValueStr, id) = DecodeCursor(cursor);
        var sortValue = JsonSerializer.Deserialize(sortValueStr, keyType);

        var parameter = Expression.Parameter(typeof(T), "e");
        var keySelectorBody = new ExpressionParameterReplacer(keySelector.Parameters[0], parameter).Visit(keySelector.Body);

        var comparisonOperator = descending ? ExpressionType.LessThan : ExpressionType.GreaterThan;
        var keyComparison = Expression.MakeBinary(comparisonOperator, keySelectorBody, Expression.Constant(sortValue, keyType));

        var idProperty = Expression.Property(parameter, nameof(IEntity.Id));
        var idComparison = Expression.MakeBinary(ExpressionType.GreaterThan, idProperty, Expression.Constant(id));

        var keyEquality = Expression.MakeBinary(ExpressionType.Equal, keySelectorBody, Expression.Constant(sortValue, keyType));

        var combinedExpression = Expression.OrElse(keyComparison, Expression.AndAlso(keyEquality, idComparison));

        var whereLambda = Expression.Lambda<Func<T, bool>>(combinedExpression, parameter);

        return (IOrderedQueryable<T>)query.Where(whereLambda);
    }

    private static string EncodeCursor(object? value, Guid id)
    {
        var cursorPayload = new { v = value, id };
        var json = JsonSerializer.Serialize(cursorPayload);
        return Convert.ToBase64String(Encoding.UTF8.GetBytes(json));
    }

    private static (string valueJson, Guid id) DecodeCursor(string cursor)
    {
        var json = Encoding.UTF8.GetString(Convert.FromBase64String(cursor));
        var doc = JsonDocument.Parse(json);
        var root = doc.RootElement;
        var id = root.GetProperty("id").GetGuid();
        var valueJson = root.GetProperty("v").GetRawText();
        return (valueJson, id);
    }

    internal class ExpressionParameterReplacer(ParameterExpression from, ParameterExpression to) : ExpressionVisitor
    {
        protected override Expression VisitParameter(ParameterExpression node)
        {
            return node == from ? to : base.VisitParameter(node);
        }
    }
}