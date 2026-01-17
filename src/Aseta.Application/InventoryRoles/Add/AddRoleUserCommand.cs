using Aseta.Application.Abstractions.Authorization;
using Aseta.Application.Abstractions.Messaging;
using Aseta.Domain.Entities.InventoryRoles;

namespace Aseta.Application.InventoryRoles.Add;

[Authorize(Role.Owner)]
public sealed record AddRoleUserCommand(Guid InventoryId, Guid UserId, Role Role) : ICommand;
