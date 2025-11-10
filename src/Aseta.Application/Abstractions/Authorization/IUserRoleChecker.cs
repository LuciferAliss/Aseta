using Aseta.Domain.Enums;

namespace Aseta.Application.Abstractions.Authorization;

public interface IUserRoleChecker
{
    Task<bool> HasAdminRoleAsync(string userId, CancellationToken cancellationToken = default);
    Task<bool> HasPermissionAsync(string userId, Guid inventoryId, Role requiredRole, CancellationToken cancellationToken = default);
}
