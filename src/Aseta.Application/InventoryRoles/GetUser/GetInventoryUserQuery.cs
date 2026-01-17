using Aseta.Application.Abstractions.Authorization;
using Aseta.Application.Abstractions.Messaging;
using Aseta.Domain.Entities.InventoryRoles;

namespace Aseta.Application.InventoryRoles.GetUser;

[Authorize(Role.Owner)]
public sealed record GetInventoryUserQuery(Guid InventoryId) : IQuery<UsersResponse>;
