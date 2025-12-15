using Aseta.Application.Abstractions.Authorization;
using Aseta.Application.Abstractions.Messaging;
using Aseta.Domain.Entities.InventoryRoles;

namespace Aseta.Application.Items.Update;

[Authorize(Role.Editor)]
public sealed record UpdateItemCommand(
    Guid ItemId,
    string CustomId,
    ICollection<CustomFieldValueData> CustomFieldsValue,
    Guid InventoryId,
    Guid UserId) : ICommand, IInventoryScopedRequest;
