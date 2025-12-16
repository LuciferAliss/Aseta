using Aseta.Application.Abstractions.Authorization;
using Aseta.Application.Abstractions.Messaging;
using Aseta.Domain.Entities.InventoryRoles;

namespace Aseta.Application.Items.Update;

[Authorize(Role.Editor)]
public sealed record UpdateItemCommand(
    Guid ItemId,
    ICollection<CustomFieldValueData> CustomFieldValues,
    Guid InventoryId,
    Guid UserId) : ICommand, IInventoryScopedRequest;
