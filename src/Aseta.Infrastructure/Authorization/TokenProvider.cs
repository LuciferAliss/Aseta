using System.Security.Claims;
using System.Text;
using Aseta.Application.Abstractions.Authorization;
using Aseta.Domain.Abstractions.Primitives.Results;
using Aseta.Domain.Entities.Users;
using Aseta.Infrastructure.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace Aseta.Infrastructure.Authorization;

internal sealed class TokenProvider(
    IOptions<JwtOptions> jwtOptions,
    IOptions<RefreshTokenOptions> refreshTokenOptions) : ITokenProvider
{
    public string CreateAccessToken(User user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Value.Secret));

        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(
            [
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email)
            ]),
            Expires = DateTime.UtcNow.AddMinutes(jwtOptions.Value.ExpirationInMinutes),
            SigningCredentials = credentials,
            Issuer = jwtOptions.Value.Issuer,
            Audience = jwtOptions.Value.Audience
        };

        var handler = new JsonWebTokenHandler();

        string token = handler.CreateToken(tokenDescriptor);

        return token;
    }

    public Result<RefreshToken> CreateRefreshToken(User user, string deviceId, string deviceName)
    {
        string token = Guid.NewGuid().ToString("N");

        Result<RefreshToken> refreshTokenResult = RefreshToken.Create(token, user.Id, DateTime.UtcNow.AddDays(refreshTokenOptions.Value.ExpirationInDays), deviceId, deviceName);

        return refreshTokenResult;
    }
}
