using Aseta.Domain.Abstractions.Repository;
using Aseta.Domain.Entities.Items;
using Aseta.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Aseta.Infrastructure.Repository;

public class ItemRepository(AppDbContext context) 
: Repository<Item>(context), IItemRepository
{
    public async Task<int> CountItems(
        Guid inventoryId,
        CancellationToken cancellationToken = default)
    {
        return await _dbSet.CountAsync(i => i.InventoryId == inventoryId, cancellationToken);
    }

    public async Task DeleteByItemIdsAsync(
        ICollection<Guid> itemIdsToRemove,
        Guid inventoryId,
        CancellationToken cancellationToken = default)
    {
        await _dbSet.Where(item =>
            item.InventoryId == inventoryId &&
            itemIdsToRemove.Contains(item.Id))
            .ExecuteDeleteAsync(cancellationToken);
    }

    public async Task<List<Item>> GetByItemsInventoryIdAsync(
        Guid inventoryId,
        CancellationToken cancellationToken = default)
    {
        return await _dbSet.Where(i =>
            i.InventoryId == inventoryId)
            .ToListAsync(cancellationToken);
    }

    public async Task<ICollection<Item>> GetItemsPageAsync(
        Guid inventoryId,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(i => i.InventoryId == inventoryId)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<int> LastItemPosition(
        Guid inventoryId,
        CancellationToken cancellationToken = default)
    {
        return await _dbSet.CountAsync(i =>
            i.InventoryId == inventoryId,
            cancellationToken);
    }
}