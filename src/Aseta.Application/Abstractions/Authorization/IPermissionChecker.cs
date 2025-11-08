using Aseta.Domain.Abstractions.Primitives;
using Aseta.Domain.Enums;

namespace Aseta.Application.Abstractions.Authorization;

public interface IPermissionChecker
{
    Task<Result<bool>> HasPermissionAsync(string userId, Guid inventoryId, Role requiredRole, CancellationToken cancellationToken);
}
