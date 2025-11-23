using Aseta.Domain.Abstractions.Persistence;
using Aseta.Domain.Entities.Categories;
using Aseta.Infrastructure.Database;
using Aseta.Infrastructure.Persistence.Common;

namespace Aseta.Infrastructure.Persistence.Categories;

internal sealed class CategoryRepository(AppDbContext context) : Repository<Category>(context), ICategoryRepository { }