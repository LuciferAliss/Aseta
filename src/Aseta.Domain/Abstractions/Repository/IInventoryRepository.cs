using System;
using Aseta.Domain.Entities.Inventories;

namespace Aseta.Domain.Abstractions.Repository;

public interface IInventoryRepository : IRepository<Inventory, Guid>
{
    Task DeleteByFieldIdsAsync(List<Guid> deletedFieldIds);
    Task<List<Inventory>> GetAllPublicInventoriesAsync(Guid userId);
}