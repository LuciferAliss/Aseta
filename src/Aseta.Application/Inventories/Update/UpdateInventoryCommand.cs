using Aseta.Application.Abstractions.Authorization;
using Aseta.Application.Abstractions.Messaging;
using Aseta.Domain.Entities.InventoryRoles;

namespace Aseta.Application.Inventories.Update;

[Authorize(Role.Owner)]
public sealed record UpdateInventoryCommand(
    Guid InventoryId,
    string Name,
    string Description,
    Uri? ImageUrl,
    Guid CategoryId,
    bool IsPublic) : ICommand, IInventoryScopedRequest;