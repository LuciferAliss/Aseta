using Aseta.Domain.Abstractions.Persistence;
using Aseta.Domain.DTO.Inventory;
using Aseta.Domain.Entities.Inventories;
using Aseta.Infrastructure.Database;
using Aseta.Infrastructure.Pagination;
using Aseta.Infrastructure.Persistence.Common;
using Microsoft.EntityFrameworkCore;

namespace Aseta.Infrastructure.Persistence.Inventories;

internal sealed class InventoryRepository(AppDbContext context) : Repository<Inventory>(context), IInventoryRepository
{
    public async Task<(ICollection<Inventory> inventories, string? nextCursor, bool hasNextPage)> GetPaginatedWithKeysetAsync(
        InventoryPaginationParameters parameters,
        CancellationToken cancellationToken)
    {
        var query = _dbSet.Include(i => i.Tags).Include(i => i.Creator).AsNoTracking();

        query = query.FilterByCreateAtFrom(parameters.CreatedAtFrom)
            .FilterByCreateAtTo(parameters.CreatedAtTo)
            .FilterByTags(parameters.TagIds)
            .FilterByCategory(parameters.CategoryIds)
            .FilterByIsPublic(parameters.IsPublic)
            .FilterByMinItemsCount(parameters.MinItemsCount)
            .FilterByMaxItemsCount(parameters.MaxItemsCount);

        var (items, nextCursor, hasNextPage) = await new KeysetPaginator<Inventory>(query)
            .AddSortableField("created_at", i => i.CreatedAt)
            .AddSortableField("items_count", i => i.ItemsCount)
            .AddSortableField("name", i => i.InventoryName)
            .AddSortableField("creator", i => i.Creator.UserName)
            .PaginateAsync("created_at", "desc", parameters.PageSize, parameters.Cursor, cancellationToken);

        return (items, nextCursor, hasNextPage);
    }   
}