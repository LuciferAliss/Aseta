using Aseta.Application.Abstractions.Authorization;
using Aseta.Application.Abstractions.Messaging;
using Aseta.Domain.Entities.Users;

namespace Aseta.Application.Categories.Delete;

[Authorize(UserRole.Admin)]
public sealed record DeleteCategoriesCommand(
    ICollection<Guid> CategoryIds) : ICommand;