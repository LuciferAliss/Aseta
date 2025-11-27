using Aseta.Application.Abstractions.Authorization;
using Aseta.Application.Abstractions.Messaging;
using Aseta.Domain.Entities.Inventories.CustomField;
using Aseta.Domain.Entities.UserRoles;

namespace Aseta.Application.Items.Create;

[Authorize(Role.Editor)]
public sealed record CreateItemCommand(
    ICollection<CustomFieldValue> CustomFieldsValue,
    Guid InventoryId,
    Guid UserId) : ICommand, IInventoryScopedRequest;