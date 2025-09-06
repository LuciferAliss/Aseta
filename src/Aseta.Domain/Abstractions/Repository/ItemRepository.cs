using System;
using Aseta.Domain.Entities.Items;

namespace Aseta.Domain.Abstractions.Repository;

public interface IItemRepository : IRepository<Item, Guid>
{
    Task<int> LastItemPosition(Guid inventoryId);
    Task<List<Item>> GetItemsPageAsync(Guid inventoryId, int pageNumber, int pageSize);
    Task<int> CountItems(Guid inventoryId);
}
