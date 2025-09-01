using System.Security.Claims;
using Aseta.Application.Abstractions.Checkers;
using Aseta.Domain.Entities.Users;
using Microsoft.AspNetCore.Identity;

namespace Aseta.Infrastructure.Checkers;

public class CheckingLockoutUser(UserManager<UserApplication> userManager) : ICheckingLockoutUser
{
    public async Task<bool> CheckAsync(ClaimsPrincipal claims)
    {
        var userId = claims.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value
            ?? throw new Exception("User not found");

        var user = await userManager.FindByIdAsync(userId.ToString())
            ?? throw new Exception("User not found");

        return await userManager.IsLockedOutAsync(user);
    }
}
