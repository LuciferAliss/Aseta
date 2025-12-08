using System;

namespace Aseta.Application.Users.Login;

public sealed record LoginResponse(string AccessToken, string RefreshToken);
