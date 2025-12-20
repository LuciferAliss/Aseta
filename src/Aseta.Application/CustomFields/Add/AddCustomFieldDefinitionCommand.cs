using Aseta.Application.Abstractions.Authorization;
using Aseta.Application.Abstractions.Messaging;
using Aseta.Domain.Entities.InventoryRoles;

namespace Aseta.Application.CustomFields.Add;

[Authorize(Role.Owner)]
public sealed record AddCustomFieldDefinitionCommand(
    Guid InventoryId,
    ICollection<CustomFieldData> NewFields) : ICommand, IInventoryScopedRequest;
