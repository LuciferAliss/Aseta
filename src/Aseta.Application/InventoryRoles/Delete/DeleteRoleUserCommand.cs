using Aseta.Application.Abstractions.Authorization;
using Aseta.Application.Abstractions.Messaging;
using Aseta.Domain.Entities.InventoryRoles;

namespace Aseta.Application.InventoryRoles.Delete;

[Authorize(Role.Owner)]
public record class DeleteRoleUserCommand(Guid InventoryId, Guid DeletedUserId) : ICommand, IInventoryScopedRequest;
