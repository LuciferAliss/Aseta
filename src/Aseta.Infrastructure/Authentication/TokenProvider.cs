using System.Security.Claims;
using System.Text;
using Aseta.Infrastructure.Options;
using Aseta.Application.Abstractions.Authentication;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Aseta.Application.Users.Contracts;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;

namespace Aseta.Infrastructure.Authentication;

internal sealed class TokenProvider(IOptions<JwtOptions> jwtOptions) : ITokenProvider
{
    private const int SIZE_REFRESH_TOKEN = 64;

    private readonly JwtOptions _jwtOptions = jwtOptions.Value;

    public string CreateAccessToken(AccessTokenGenerationRequest request)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Secret));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(
            [
                new Claim(JwtRegisteredClaimNames.Sub, request.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, request.Email)
            ]),
            Expires = DateTime.UtcNow.AddMinutes(_jwtOptions.ExpirationInMinutes),
            SigningCredentials = credentials,
            Issuer = _jwtOptions.Issuer,
            Audience = _jwtOptions.Audience
        };

        var handler = new JsonWebTokenHandler();

        return handler.CreateToken(tokenDescriptor);
    }

    public string CreateRefreshToken()
    {
        byte[] rt = new byte[SIZE_REFRESH_TOKEN];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(rt);
        return Convert.ToBase64String(rt);
    }
}
