using Aseta.Application.Abstractions.Services;
using Aseta.Application.DTO.User;
using Aseta.Domain.Entities.Users;
using Microsoft.AspNetCore.Identity;

namespace Aseta.Application.Services;

public class AuthService(
    UserManager<UserApplication> userManager
) : IAuthService
{
    private readonly UserManager<UserApplication> _userManager = userManager;

    public async Task<UserResponse> GetCurrentUserAsync(Guid userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString())
            ?? throw new Exception("User not found");

        var response = new UserResponse(user.Id, user.Email, await _userManager.IsInRoleAsync(user, "Admin") ? "Admin" : "User");

        return response;
    }
}
