using Aseta.Domain.Entities.Items;

namespace Aseta.Infrastructure.Persistence.Items;

internal static class ItemQueryExtensions
{
    public static IQueryable<Item> FilterByCreateAtTo(this IQueryable<Item> query, DateTime? createdAtTo)
    {
        if (!createdAtTo.HasValue)
        {
            return query;
        }

        return query.Where(i => i.CreatedAt <= createdAtTo.Value);
    }

    public static IQueryable<Item> FilterByCreateAtFrom(this IQueryable<Item> query, DateTime? createdAtFrom)
    {
        if (!createdAtFrom.HasValue)
        {
            return query;
        }

        return query.Where(i => i.CreatedAt >= createdAtFrom.Value);
    }

    public static IQueryable<Item> FilterByUpdateAtTo(this IQueryable<Item> query, DateTime? updatedAtTo)
    {
        if (!updatedAtTo.HasValue)
        {
            return query;
        }

        return query.Where(i => i.UpdatedAt <= updatedAtTo.Value);
    }

    public static IQueryable<Item> FilterByUpdateAtFrom(this IQueryable<Item> query, DateTime? updatedAtFrom)
    {
        if (!updatedAtFrom.HasValue)
        {
            return query;
        }

        return query.Where(i => i.UpdatedAt >= updatedAtFrom.Value);
    }

    public static IQueryable<Item> FilterByUpdater(this IQueryable<Item> query, Guid? updaterId)
    {
        if (updaterId.HasValue)
        {
            return query;
        }

        return query.Where(i => i.UpdaterId == updaterId);
    }

    public static IQueryable<Item> FilterByCreator(this IQueryable<Item> query, Guid? creatorId)
    {
        if (creatorId.HasValue)
        {
            return query;
        }

        return query.Where(i => i.CreatorId == creatorId);
    }
}