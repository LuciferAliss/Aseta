using System;
using Aseta.Domain.Abstractions.Repository;
using Aseta.Domain.Entities.Items;
using Aseta.Infrastructure.Database;

namespace Aseta.Infrastructure.Repository;

public class ItemRepository(AppDbContext context) : Repository<Item>(context), IItemRepository { }