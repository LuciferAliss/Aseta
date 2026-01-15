using Aseta.Domain.Entities.Tags;

namespace Aseta.Domain.Abstractions.Persistence;

public interface ITagRepository : IRepository<Tag>
{
    Task<Tag?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
}