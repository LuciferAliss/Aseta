using Aseta.Domain.Entities.Tags;

namespace Aseta.Domain.Abstractions.Repository;

public interface ITagRepository : IRepository<Tag, int>
{
    Task<Tag?> GetByNameAsync(string name);
    Task<bool> ExistsByNameAsync(string name);
    IQueryable<Tag> GetAllAsQueryable();
}
