using Aseta.Domain.Abstractions.Persistence;
using Aseta.Domain.Entities.Inventories;
using Aseta.Infrastructure.Database;

namespace Aseta.Infrastructure.Repository;

internal sealed class CategoryRepository(AppDbContext context) : Repository<Category>(context), ICategoryRepository { }