using System;
using Aseta.Domain.Abstractions.Primitives.Results;
using Aseta.Domain.Entities.Users;

namespace Aseta.Application.Abstractions.Authorization;

public interface ITokenProvider
{
    string CreateAccessToken(User user);
    Result<RefreshToken> CreateRefreshToken(User user, string deviceId, string deviceName);
}
