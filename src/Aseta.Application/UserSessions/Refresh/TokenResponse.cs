using Aseta.Domain.Entities.Users;

namespace Aseta.Application.UserSessions.Refresh;

public sealed record TokenResponse(string AccessToken, UserSession UserSession);
