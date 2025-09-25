using System.Security.Claims;

namespace Aseta.Domain.Abstractions.Checkers;

public interface ICheckingAccessPolicy
{
    Task<bool> CheckAsync(ClaimsPrincipal claims, Guid inventoryId, string policyName);
}
