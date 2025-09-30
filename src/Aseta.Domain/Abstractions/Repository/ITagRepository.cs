using Aseta.Domain.Entities.Tags;

namespace Aseta.Domain.Abstractions.Repository;

public interface ITagRepository : IRepository<Tag>
{
    Task<Tag?> GetByNameAsync(string name, CancellationToken cancellationToken = default);

    Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default);
    
    Task<ICollection<Tag>> GetByNamesAsync(
        ICollection<string> requestedTagNames,
        CancellationToken cancellationToken = default);

    Task AddTagsAsync(ICollection<Tag> tags, CancellationToken cancellationToken = default);
}
