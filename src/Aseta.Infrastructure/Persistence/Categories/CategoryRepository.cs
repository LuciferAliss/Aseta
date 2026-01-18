using Aseta.Domain.Abstractions.Persistence;
using Aseta.Domain.Entities.Categories;
using Aseta.Infrastructure.Database;
using Aseta.Infrastructure.Persistence.Common;

namespace Aseta.Infrastructure.Persistence.Categories;

public sealed class CategoryRepository(AppDbContext context) : Repository<Category>(context), ICategoryRepository
{
    public Task<Category?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return FirstOrDefaultAsync(i => i.Name == name, cancellationToken: cancellationToken);
    }
}