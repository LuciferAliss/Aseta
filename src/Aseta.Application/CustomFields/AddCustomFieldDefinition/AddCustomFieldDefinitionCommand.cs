using Aseta.Application.Abstractions.Authorization;
using Aseta.Application.Abstractions.Messaging;
using Aseta.Domain.Entities.InventoryRoles;

namespace Aseta.Application.CustomFields.AddCustomFieldDefinition;

[Authorize(Role.Owner)]
public sealed record AddCustomFieldDefinitionCommand(
    Guid InventoryId,
    ICollection<CustomFieldDefinitionData> NewFields) : ICommand, IInventoryScopedRequest;
