using Aseta.Application.Users.Contracts;
using Aseta.Application.Users.Login;
using Aseta.Application.Users.Register;
using Microsoft.AspNetCore.Mvc;

namespace Aseta.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(IRegisterService registerService, ILoginService loginService) : ControllerBase
{
    private readonly IRegisterService _registerService = registerService;
    private readonly ILoginService _loginService = loginService;

    [HttpPost("register")]
    public async Task<IActionResult> Register(UserRegisterRequest request, CancellationToken cancellationToken)
    {
        await _registerService.RegisterUserAsync(request, cancellationToken);

        return Ok();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(UserLoginRequest request, CancellationToken cancellationToken)
    {
        var response = await _loginService.LoginUserAsync(request, cancellationToken);

        return Ok(response);
    }
}