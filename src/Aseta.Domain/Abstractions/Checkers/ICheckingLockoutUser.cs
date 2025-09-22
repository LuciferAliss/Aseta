using System.Security.Claims;

namespace Aseta.Application.Abstractions.Checkers;

public interface ICheckingLockoutUser
{
    Task CheckAsync(ClaimsPrincipal claims);
}
