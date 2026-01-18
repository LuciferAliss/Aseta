using Aseta.Application.Abstractions.Authorization;
using Aseta.Application.Abstractions.Messaging;
using Aseta.Domain.Entities.Users;

namespace Aseta.Application.Tags.Delete;

[Authorize(UserRole.Admin)]
public sealed record DeleteTagsCommand(
    ICollection<Guid> TagIds) : ICommand;