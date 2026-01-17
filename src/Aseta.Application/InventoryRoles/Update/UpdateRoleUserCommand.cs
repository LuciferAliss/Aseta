using Aseta.Application.Abstractions.Authorization;
using Aseta.Application.Abstractions.Messaging;
using Aseta.Domain.Entities.InventoryRoles;

namespace Aseta.Application.InventoryRoles.Update;

[Authorize(Role.Owner)]
public sealed record UpdateRoleUserCommand(Guid InventoryId, Guid UserId, Role Role) : ICommand;
