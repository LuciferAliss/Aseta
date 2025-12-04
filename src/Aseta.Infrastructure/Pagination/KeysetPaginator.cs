using Aseta.Domain.Abstractions.Primitives.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace Aseta.Infrastructure.Pagination;

internal sealed class KeysetPaginator<T>(IQueryable<T> query) where T : class, IEntity
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
        if (!_sortFields.TryGetValue(sortBy, out (LambdaExpression keySelector, Type keyType) sortInfo))
        {
            throw new ArgumentException($"Sort key '{sortBy}' is not registered.", nameof(sortBy));
        }

        bool isDescending = sortOrder.Equals("desc", StringComparison.OrdinalIgnoreCase);

        IOrderedQueryable<T> orderedQuery = ApplyOrderBy(sortInfo.keySelector, isDescending);

        if (!string.IsNullOrWhiteSpace(cursor))
        {
            orderedQuery = ApplyCursorWhere(orderedQuery, cursor, sortInfo.keySelector, sortInfo.keyType, isDescending);
        }

        List<T> items = await orderedQuery.Take(pageSize + 1).ToListAsync(cancellationToken);

        bool hasNextPage = items.Count > pageSize;
        var pageItems = items.Take(pageSize).ToList();
        string? nextCursor = null;

        if (hasNextPage && pageItems.Count != 0)
        {
            T lastItem = pageItems[pageItems.Count - 1];
            object? sortValue = sortInfo.keySelector.Compile().DynamicInvoke(lastItem);
            nextCursor = EncodeCursor(sortValue, lastItem.Id);
        }

        return (pageItems, nextCursor, hasNextPage);
    }

    private IOrderedQueryable<T> ApplyOrderBy(LambdaExpression keySelector, bool descending)
    {
        string methodName = descending ? nameof(Queryable.OrderByDescending) : nameof(Queryable.OrderBy);

        MethodInfo method = typeof(Queryable).GetMethods()
            .First(m => m.Name == methodName && m.GetParameters().Length == 2)
            .MakeGenericMethod(typeof(T), keySelector.Body.Type);

        var sortedQuery = (IOrderedQueryable<T>)method.Invoke(null, [query, keySelector])!;

        return sortedQuery.ThenBy(e => e.Id);
    }

    private IOrderedQueryable<T> ApplyCursorWhere(IOrderedQueryable<T> query, string cursor, LambdaExpression keySelector, Type keyType, bool descending)
    {
        (string? sortValueStr, Guid id) = DecodeCursor(cursor);
        object? sortValue = JsonSerializer.Deserialize(sortValueStr, keyType);

        ParameterExpression parameter = Expression.Parameter(typeof(T), "e");
        Expression keySelectorBody = new ExpressionParameterReplacer(keySelector.Parameters[0], parameter).Visit(keySelector.Body);

        ExpressionType comparisonOperator = descending ? ExpressionType.LessThan : ExpressionType.GreaterThan;
        BinaryExpression keyComparison = Expression.MakeBinary(comparisonOperator, keySelectorBody, Expression.Constant(sortValue, keyType));

        MemberExpression idProperty = Expression.Property(parameter, nameof(IEntity.Id));
        BinaryExpression idComparison = Expression.MakeBinary(ExpressionType.GreaterThan, idProperty, Expression.Constant(id));

        BinaryExpression keyEquality = Expression.MakeBinary(ExpressionType.Equal, keySelectorBody, Expression.Constant(sortValue, keyType));

        BinaryExpression combinedExpression = Expression.OrElse(keyComparison, Expression.AndAlso(keyEquality, idComparison));

        var whereLambda = Expression.Lambda<Func<T, bool>>(combinedExpression, parameter);

        return (IOrderedQueryable<T>)query.Where(whereLambda);
    }

    private static string EncodeCursor(object? value, Guid id)
    {
        var cursorPayload = new { v = value, id };
        string json = JsonSerializer.Serialize(cursorPayload);
        return Convert.ToBase64String(Encoding.UTF8.GetBytes(json));
    }

    private static (string valueJson, Guid id) DecodeCursor(string cursor)
    {
        string json = Encoding.UTF8.GetString(Convert.FromBase64String(cursor));
        var doc = JsonDocument.Parse(json);
        JsonElement root = doc.RootElement;
        Guid id = root.GetProperty("id").GetGuid();
        string valueJson = root.GetProperty("v").GetRawText();
        return (valueJson, id);
    }

    internal sealed class ExpressionParameterReplacer(ParameterExpression from, ParameterExpression to) : ExpressionVisitor
    {
        protected override Expression VisitParameter(ParameterExpression node)
        {
            return node == from ? to : base.VisitParameter(node);
        }
    }
}