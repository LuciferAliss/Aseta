using Aseta.Application.Users.Contracts;

namespace Aseta.Application.Users.Login;

public interface ILoginService
{
    Task<AuthTokenResponse> LoginUserAsync(UserLoginRequest request, CancellationToken cancellationToken);
}