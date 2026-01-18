using Aseta.Application.Abstractions.Authorization;
using Aseta.Application.Abstractions.Messaging;
using Aseta.Domain.Entities.Users;

namespace Aseta.Application.Categories.Update;

[Authorize(UserRole.Admin)]
public sealed record UpdateCategoryCommand(
    Guid CategoryId,
    string Name) : ICommand;
