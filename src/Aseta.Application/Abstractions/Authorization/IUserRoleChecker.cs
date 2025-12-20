using Aseta.Domain.Entities.InventoryRoles;

namespace Aseta.Application.Abstractions.Authorization;

public interface IUserRoleChecker
{
    Task<bool> HasPermissionAsync(Guid userId, Guid inventoryId, Role requiredRole, CancellationToken cancellationToken = default);
}
