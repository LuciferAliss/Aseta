using System;
using Aseta.Domain.Abstractions.Repository;
using Aseta.Domain.Entities.Tags;
using Aseta.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Aseta.Infrastructure.Repository;

public class TagRepository(AppDbContext context) : Repository<Tag, int>(context), ITagRepository
{
    public async Task<Tag?> GetByNameAsync(string name)
    {
        return await _dbSet.FirstOrDefaultAsync(t => t.Name == name);
    }

    public async Task<bool> ExistsByNameAsync(string name)
    {
        return await _dbSet.AnyAsync(t => t.Name == name);
    }

    public IQueryable<Tag> GetAllAsQueryable()
    {
        return _dbSet.AsQueryable();
    }
}
