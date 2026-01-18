using Aseta.Application.Abstractions.Authorization;
using Aseta.Application.Abstractions.Messaging;
using Aseta.Domain.Entities.Users;

namespace Aseta.Application.Categories.Create;

[Authorize(UserRole.Admin)]
public sealed record CreateCategoryCommand(string Name) : ICommand;