using System;
using Aseta.Domain.Entities.Users;

namespace Aseta.Application.UserSessions.Login;

public sealed record LoginResponse(string AccessToken, UserSession UserSession);
