using Aseta.Application.Abstractions.Authentication;
using Aseta.Application.Users.Contracts;
using Aseta.Application.Users.Exceptions;
using Aseta.Domain.User;
using Microsoft.AspNetCore.Identity;

namespace Aseta.Application.Users.Login;

internal sealed class LoginService(ITokenProvider tokenProvider, UserManager<User> userManager) : ILoginService
{
    private readonly ITokenProvider _tokenProvider = tokenProvider;
    private readonly UserManager<User> _userManager = userManager;

    public async Task<AuthTokenResponse> LoginUserAsync(UserLoginRequest request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByNameAsync(request.UserName).WaitAsync(cancellationToken) ?? throw new NotFoundException($"User with name '{request.UserName}' not found.");

        var result = await _userManager.CheckPasswordAsync(user, request.Password).WaitAsync(cancellationToken);

        if (!result)
            throw new InvalidPasswordException($"Invalid password for user '{request.UserName}'.");

        var accessToken = _tokenProvider.CreateAccessToken(new AccessTokenGenerationRequest(user.Id, user.Email!));
        var refreshToken = _tokenProvider.CreateRefreshToken();

        
        return new AuthTokenResponse(accessToken, refreshToken);
    }
}