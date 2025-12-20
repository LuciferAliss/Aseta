using Aseta.Application.Abstractions.Messaging;

namespace Aseta.Application.UserSessions.Refresh;

public sealed record RefreshUserTokensCommand(string RefreshToken) : ICommand<TokenResponse>;