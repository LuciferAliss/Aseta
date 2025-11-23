using Aseta.Domain.Entities.UserRoles;

namespace Aseta.Application.Abstractions.Authorization;

public interface IUserRoleChecker
{
    Task<bool> HasPermissionAsync(string userId, Guid inventoryId, Role requiredRole, CancellationToken cancellationToken = default);
}
