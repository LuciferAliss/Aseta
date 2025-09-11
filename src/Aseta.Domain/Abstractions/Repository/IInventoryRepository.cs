using System;
using Aseta.Domain.Entities.Inventories;

namespace Aseta.Domain.Abstractions.Repository;

public interface IInventoryRepository : IRepository<Inventory, Guid>
{
    Task<int> CountAsync();
    Task<List<Inventory>> GetLastInventoriesPageAsync(int pageNumber, int pageSize);
    Task DeleteByFieldIdsAsync(List<Guid> deletedFieldIds);
    Task<List<Inventory>> GetMostPopularInventoriesAsync(int itemCount);
    Task<Inventory?> GetByIdWithTagsAsync(Guid id);
    Task<bool> ExistsAsync(Guid id);
}