using System;
using Aseta.Application.Abstractions.Authorization;
using Aseta.Application.Abstractions.Messaging;
using Aseta.Domain.Entities.Users;

namespace Aseta.Application.Tags.Update;

[Authorize(UserRole.Admin)]
public sealed record UpdateTagCommand(Guid Id, string Name) : ICommand;
