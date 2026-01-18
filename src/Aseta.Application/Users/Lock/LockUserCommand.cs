using Aseta.Application.Abstractions.Authorization;
using Aseta.Application.Abstractions.Messaging;
using Aseta.Domain.Entities.Users;

namespace Aseta.Application.Users.Lock;

[Authorize(UserRole.Admin)]
public sealed record LockUserCommand(Guid UserId) : ICommand;
