using Aseta.Application.Abstractions.Authorization;
using Aseta.Application.Abstractions.Messaging;
using Aseta.Domain.Entities.UserRoles;

namespace Aseta.Application.Inventories.Delete;

[Authorize(Role.Owner)]
public sealed record DeleteInventoryCommand(Guid InventoryId) : ICommand, IInventoryScopedRequest;