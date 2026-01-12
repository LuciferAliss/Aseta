using Aseta.Application.Abstractions.Messaging;

namespace Aseta.Application.UserSessions.Logout;

public sealed record LogoutUserCommand(string RefreshToken) : ICommand;
