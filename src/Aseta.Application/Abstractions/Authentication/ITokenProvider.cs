using System;
using Aseta.Application.Users.Contracts;

namespace Aseta.Application.Abstractions.Authentication;

public interface ITokenProvider
{
    string CreateAccessToken(AccessTokenGenerationRequest userData);
    string CreateRefreshToken();
}
