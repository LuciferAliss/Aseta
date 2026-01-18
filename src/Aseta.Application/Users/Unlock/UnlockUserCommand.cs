using Aseta.Application.Abstractions.Authorization;
using Aseta.Application.Abstractions.Messaging;
using Aseta.Domain.Entities.Users;

namespace Aseta.Application.Users.Unlock;

[Authorize(UserRole.Admin)]
public sealed record UnlockUserCommand(Guid UserId) : ICommand;
