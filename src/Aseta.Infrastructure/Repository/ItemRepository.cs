using Aseta.Domain.Abstractions.Persistence;
using Aseta.Domain.Entities.Items;
using Aseta.Infrastructure.Database;

namespace Aseta.Infrastructure.Repository;

public class ItemRepository(AppDbContext context) : Repository<Item>(context), IItemRepository;