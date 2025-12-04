using Aseta.Application.Abstractions.Authorization;
using Aseta.Application.Abstractions.Messaging;
using Aseta.Domain.Entities.UserRoles;

namespace Aseta.Application.Inventories.Update;

[Authorize(Role.Owner)]
public sealed record UpdateInventoryCommand(
    Guid InventoryId,
    string Name,
    string Description,
    Uri ImageUrl,
    bool IsPublic) : ICommand, IInventoryScopedRequest;