using System.Security.Claims;
using Aseta.Domain.Entities.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Aseta.API.Controllers;

[Route("[controller]")]
[ApiController]
public class AuthController(SignInManager<UserApplication> signInManager) : ControllerBase
{
    private readonly SignInManager<UserApplication> signInManager = signInManager;

    [HttpGet("pingauth")]
    [Authorize]
    public ActionResult PingAuth()
    {
        var email = User.FindFirstValue(ClaimTypes.Email);
        return Ok(new { Email = email });
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task<ActionResult> Logout()
    {
        await signInManager.SignOutAsync();
        return Ok();
    }
}