using Aseta.Domain.Abstractions.Persistence;
using Aseta.Domain.DTO.Items;
using Aseta.Domain.Entities.Items;
using Aseta.Infrastructure.Database;
using Aseta.Infrastructure.Pagination;
using Aseta.Infrastructure.Persistence.Common;
using Microsoft.EntityFrameworkCore;

namespace Aseta.Infrastructure.Persistence.Items;

public sealed class ItemRepository(AppDbContext context) : Repository<Item>(context), IItemRepository
{
    public async Task<int> GetItemSequenceNumberAsync(Guid itemId, Guid inventoryId, CancellationToken cancellationToken = default)
    {
        var result = await _dbSet
            .ApplyTracking(false)
            .Where(i => i.Id == itemId && i.InventoryId == inventoryId)
            .Select(item => new
            {
                Sequence = _dbSet.Count(preceding =>
                    preceding.InventoryId == inventoryId &&
                    preceding.CreatedAt < item.CreatedAt)
            })
            .FirstOrDefaultAsync(cancellationToken);

        return result is null ? 0 : result.Sequence + 1;
    }

    public async Task<(ICollection<Item> items, string? nextCursor, bool hasNextPage)> GetPaginatedWithKeysetAsync(
        ItemPaginationParameters parameters,
        CancellationToken cancellationToken = default)
    {
        IQueryable<Item> query = _dbSet.ApplyInclude(i => i.Creator, i => i.Updater)
            .ApplyTracking(false);

        query = query.FilterByCreateAtFrom(parameters.CreatedAtFrom)
            .FilterByCreateAtTo(parameters.CreatedAtTo)
            .FilterByUpdateAtFrom(parameters.UpdatedAtFrom)
            .FilterByUpdateAtTo(parameters.UpdatedAtTo)
            .FilterByCreator(parameters.CreatorId)
            .FilterByUpdater(parameters.UpdaterId);

        (ICollection<Item>? items, string? nextCursor, bool hasNextPage) = await new KeysetPaginator<Item>(query)
            .AddSortableField(SortBy.DateCreated.ToString(), i => i.CreatedAt)
            .AddSortableField(SortBy.DateUpdated.ToString(), i => i.UpdatedAt)
            .AddSortableField(SortBy.Creator.ToString(), i => i.Creator.UserName)
            .AddSortableField(SortBy.Updater.ToString(), i => i.Updater.UserName)
            .PaginateAsync(parameters.SortBy.ToString(), parameters.SortOrder, parameters.PageSize, parameters.Cursor, cancellationToken);

        return (items, nextCursor, hasNextPage);
    }
}