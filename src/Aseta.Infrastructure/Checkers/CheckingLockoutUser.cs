using System.Security.Claims;
using Aseta.Application.Abstractions.Checkers;
using Aseta.Domain.Entities.Users;
using Microsoft.AspNetCore.Identity;

namespace Aseta.Infrastructure.Checkers;

public class CheckingLockoutUser(UserManager<UserApplication> userManager) : ICheckingLockoutUser
{
    public async Task CheckAsync(ClaimsPrincipal claims)
    {
        var userId = claims.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            return;
        }

        var user = await userManager.FindByIdAsync(userId.ToString());
        if (user == null)
        {
            return;
        }

        if (await userManager.IsLockedOutAsync(user))
        {
            throw new Exception("User is lockout");
        }
    }
}
