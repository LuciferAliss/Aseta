using System.Linq.Expressions;
using Aseta.Domain.Abstractions.Persistence;
using Aseta.Domain.Entities.Users;
using Aseta.Infrastructure.Database;
using Aseta.Infrastructure.Persistence.Common;
using Microsoft.EntityFrameworkCore;

namespace Aseta.Infrastructure.Persistence.Users;

public sealed class UserRepository(AppDbContext context) : Repository<User>(context), IUserRepository
{
    public async Task<User?> GetByEmailAsync(
        string email,
        bool trackChanges = false,
        CancellationToken cancellationToken = default,
        params Expression<Func<User, object>>[] includeProperties)
    {
        return await _dbSet.ApplyInclude(includeProperties)
            .ApplyTracking(trackChanges)
            .FirstOrDefaultAsync(i => i.Email == email, cancellationToken: cancellationToken);
    }

    public async Task<ICollection<User>?> SearchAsync(
        string email,
        bool trackChanges = false,
        CancellationToken cancellationToken = default,
        params Expression<Func<User, object>>[] includeProperties)
    {
        return await _dbSet.Where(u => u.Email.Contains(
            email,
            StringComparison.CurrentCultureIgnoreCase))
            .ToListAsync(cancellationToken);

        throw new NotImplementedException();
    }
}