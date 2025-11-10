using Aseta.Domain.Abstractions.Persistence;
using Aseta.Domain.Entities.Inventories;
using Aseta.Infrastructure.Database;

namespace Aseta.Infrastructure.Repository;

internal sealed class InventoryRepository(AppDbContext context) : Repository<Inventory>(context), IInventoryRepository;