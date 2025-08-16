using System;

namespace Aseta.Application.Users.Contracts;

public sealed record AuthTokenResponse(string AccessToken, string RefreshToken);