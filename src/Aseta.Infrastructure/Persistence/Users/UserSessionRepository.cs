using Aseta.Domain.Abstractions.Persistence;
using Aseta.Domain.Entities.Users;
using Aseta.Infrastructure.Database;
using Aseta.Infrastructure.Persistence.Common;
using Microsoft.EntityFrameworkCore;

namespace Aseta.Infrastructure.Persistence.Users;

public sealed class UserSessionRepository(AppDbContext context) : Repository<UserSession>(context), IUserSessionRepository
{
    public Task<UserSession?> GetByRefreshTokenAsync(string refreshToken)
    {
        return _dbSet
            .Include(x => x.User)
            .FirstOrDefaultAsync(s => s.Token == refreshToken);
    }

    public async Task RevokeAllForUserAsync(Guid userId)
    {
        await _dbSet
            .Where(s => s.UserId == userId && !s.IsRevoked)
            .ForEachAsync(s => s.Revoke());
    }
}