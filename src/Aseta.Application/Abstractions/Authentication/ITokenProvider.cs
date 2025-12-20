using System;
using Aseta.Domain.Abstractions.Primitives.Results;
using Aseta.Domain.Entities.Users;

namespace Aseta.Application.Abstractions.Authentication;

public interface ITokenProvider
{
    string CreateAccessToken(User user, Guid sessionId);
    (string token, int expiresIn) CreateRefreshToken();
}
