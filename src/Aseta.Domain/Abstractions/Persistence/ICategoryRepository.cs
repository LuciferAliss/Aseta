using Aseta.Domain.Entities.Categories;

namespace Aseta.Domain.Abstractions.Persistence;

public interface ICategoryRepository : IRepository<Category>
{
    Task<Category?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
}