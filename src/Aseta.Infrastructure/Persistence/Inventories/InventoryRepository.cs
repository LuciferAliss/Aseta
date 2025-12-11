using Aseta.Domain.Abstractions.Persistence;
using Aseta.Domain.DTO.Inventories;
using Aseta.Domain.Entities.Inventories;
using Aseta.Infrastructure.Database;
using Aseta.Infrastructure.Pagination;
using Aseta.Infrastructure.Persistence.Common;

namespace Aseta.Infrastructure.Persistence.Inventories;

public sealed class InventoryRepository(AppDbContext context) : Repository<Inventory>(context), IInventoryRepository
{
    public async Task<(ICollection<Inventory> inventories, string? nextCursor, bool hasNextPage)> GetPaginatedWithKeysetAsync(
        InventoryPaginationParameters parameters,
        CancellationToken cancellationToken)
    {
        IQueryable<Inventory> query = _dbSet.ApplyInclude(i => i.Tags, i => i.Creator, i => i.Category, i => i.UserRoles)
            .ApplyTracking(false);

        query = query.FilterByCreateAtFrom(parameters.CreatedAtFrom)
            .FilterByCreateAtTo(parameters.CreatedAtTo)
            .FilterByTags(parameters.TagIds)
            .FilterByCategory(parameters.CategoryIds)
            .FilterByMinItemsCount(parameters.MinItemsCount)
            .FilterByMaxItemsCount(parameters.MaxItemsCount);

        (ICollection<Inventory>? items, string? nextCursor, bool hasNextPage) = await new KeysetPaginator<Inventory>(query)
            .AddSortableField(SortBy.Date.ToString(), i => i.CreatedAt)
            .AddSortableField(SortBy.NumberOfItems.ToString(), i => i.ItemsCount)
            .AddSortableField(SortBy.Name.ToString(), i => i.Name)
            .AddSortableField(SortBy.Creator.ToString(), i => i.Creator.UserName)
            .PaginateAsync(parameters.SortBy.ToString(), parameters.SortOrder, parameters.PageSize, parameters.Cursor, cancellationToken);

        return (items, nextCursor, hasNextPage);
    }
}