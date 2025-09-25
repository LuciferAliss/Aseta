using System.Security.Claims;

namespace Aseta.Domain.Abstractions.Checkers;

public interface ICheckingLockoutUser
{
    Task CheckAsync(ClaimsPrincipal claims);
}
