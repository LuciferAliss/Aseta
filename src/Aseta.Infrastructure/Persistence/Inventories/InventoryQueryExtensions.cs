using Aseta.Domain.Entities.Inventories;

namespace Aseta.Infrastructure.Persistence.Inventories;

internal static class InventoryQueryExtensions
{
    public static IQueryable<Inventory> FilterByCreateAtFrom(this IQueryable<Inventory> query, DateTime? createdAtFrom)
    {
        if (!createdAtFrom.HasValue)
        {
            return query;
        }


        return query.Where(i => i.CreatedAt >= createdAtFrom.Value);
    }

    public static IQueryable<Inventory> FilterByCreateAtTo(this IQueryable<Inventory> query, DateTime? createdAtTo)
    {
        if (!createdAtTo.HasValue)
        {
            return query;
        }


        return query.Where(i => i.CreatedAt <= createdAtTo.Value);
    }

    public static IQueryable<Inventory> FilterByTags(this IQueryable<Inventory> query, ICollection<Guid> tagIds)
    {
        if (tagIds.Count == 0)
        {
            return query;
        }


        return query.Where(i => i.Tags.Any(t => tagIds.Contains(t.Id)));
    }

    public static IQueryable<Inventory> FilterByCategory(this IQueryable<Inventory> query, ICollection<Guid> categoryIds)
    {
        if (categoryIds.Count == 0)
        {
            return query;
        }


        return query.Where(i => categoryIds.Contains(i.CategoryId));
    }

    public static IQueryable<Inventory> FilterByIsPublic(this IQueryable<Inventory> query, bool? isPublic)
    {
        if (!isPublic.HasValue)
        {
            return query;
        }


        return query.Where(i => i.IsPublic == isPublic.Value);
    }

    public static IQueryable<Inventory> FilterByMinItemsCount(this IQueryable<Inventory> query, int? minItemsCount)
    {
        if (!minItemsCount.HasValue)
        {
            return query;
        }


        return query.Where(i => i.ItemsCount >= minItemsCount.Value);
    }

    public static IQueryable<Inventory> FilterByMaxItemsCount(this IQueryable<Inventory> query, int? maxItemsCount)
    {
        if (!maxItemsCount.HasValue)
        {
            return query;
        }


        return query.Where(i => i.ItemsCount <= maxItemsCount.Value);
    }
}