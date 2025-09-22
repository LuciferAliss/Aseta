using System.Security.Claims;
using Aseta.Domain.Abstractions.Services;
using Aseta.Domain.Entities.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Aseta.API.Controllers;

[Route("[controller]")]
[ApiController]
public class AuthController(IAuthService authService, SignInManager<UserApplication> signInManager) : ControllerBase
{
    private readonly IAuthService _authService = authService;
    private readonly SignInManager<UserApplication> _signInManager = signInManager;

    [HttpGet("pingauth")]
    [Authorize]
    public async Task<ActionResult> PingAuth()
    {
        var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value
            ?? throw new Exception("User not found");

        var response = await _authService.GetCurrentUserAsync(Guid.Parse(userId));

        return Ok(response);
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task<ActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return Ok();
    }
}