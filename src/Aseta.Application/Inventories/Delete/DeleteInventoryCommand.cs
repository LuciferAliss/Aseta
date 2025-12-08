using Aseta.Application.Abstractions.Authorization;
using Aseta.Application.Abstractions.Messaging;
using Aseta.Domain.Entities.InventoryRoles;

namespace Aseta.Application.Inventories.Delete;

[Authorize(Role.Owner)]
public sealed record DeleteInventoryCommand(Guid InventoryId) : ICommand, IInventoryScopedRequest;