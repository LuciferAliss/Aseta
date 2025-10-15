using Aseta.Application.Abstractions.Authorization;
using Aseta.Application.Abstractions.Messaging;
using Aseta.Domain.Enums;

namespace Aseta.Application.Inventories.Create;

[Authorize(Role = Role.None, Permission = Permission.None)]
public sealed record CreateInventoryCommand(
    string Name,
    string Description,
    string ImageUrl,
    bool IsPublic,
    int CategoryId,
    Guid UserId) : ICommand<InventoryResponse>;