using Aseta.Domain.Abstractions.Repository;
using Aseta.Domain.Entities.Tags;
using Aseta.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Aseta.Infrastructure.Repository;

public class TagRepository(AppDbContext context) : Repository<Tag>(context), ITagRepository
{
    public async Task<Tag?> GetByNameAsync(
        string name,
        CancellationToken cancellationToken = default)
    {
        return await _dbSet.FirstOrDefaultAsync(t => t.Name == name, cancellationToken);
    }

    public async Task<bool> ExistsByNameAsync(
        string name,
        CancellationToken cancellationToken = default)
    {
        return await _dbSet.AnyAsync(t => t.Name == name, cancellationToken);
    }

    public async Task<ICollection<Tag>> GetByNamesAsync(
        ICollection<string> requestedTagNames,
        CancellationToken cancellationToken = default)
    {
        return await _dbSet.Where(t =>
            requestedTagNames.Contains(t.Name))
            .ToListAsync(cancellationToken);
    }

    public async Task AddTagsAsync(
        ICollection<Tag> tags,
        CancellationToken cancellationToken = default)
    {
        await _dbSet.AddRangeAsync(tags, cancellationToken);
    }
}
