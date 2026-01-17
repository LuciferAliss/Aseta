using Aseta.Application.Abstractions.Authorization;
using Aseta.Application.Abstractions.Messaging;
using Aseta.Domain.Entities.Users;

namespace Aseta.Application.Tags.Create;

[Authorize(UserRole.Admin)]
public sealed record CreateTagCommand(string Name) : ICommand;
