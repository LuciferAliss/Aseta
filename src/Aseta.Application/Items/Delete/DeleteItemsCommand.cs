using Aseta.Application.Abstractions.Authorization;
using Aseta.Application.Abstractions.Messaging;
using Aseta.Domain.Entities.InventoryRoles;

namespace Aseta.Application.Items.Delete;

[Authorize(Role.Editor)]
public sealed record DeleteItemsCommand(
    ICollection<Guid> ItemIds,
    Guid InventoryId) : ICommand, IInventoryScopedRequest;