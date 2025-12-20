using Aseta.Domain.Entities.Users;

namespace Aseta.Domain.Abstractions.Persistence;

public interface IUserSessionRepository : IRepository<UserSession>
{
    Task<UserSession?> GetByRefreshTokenAsync(string refreshToken);
    Task RevokeAllForUserAsync(Guid userId);
}