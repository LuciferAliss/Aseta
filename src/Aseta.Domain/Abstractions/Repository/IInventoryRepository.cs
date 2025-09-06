using System;
using Aseta.Domain.Entities.Inventories;

namespace Aseta.Domain.Abstractions.Repository;

public interface IInventoryRepository : IRepository<Inventory, Guid>
{
    Task<int> CountPublicInventoriesAsync();
    Task DeleteByFieldIdsAsync(List<Guid> deletedFieldIds);
    Task<List<Inventory>> GetPublicInventoriesPageAsync(Guid userId, int pageNumber, int pageSize);
}