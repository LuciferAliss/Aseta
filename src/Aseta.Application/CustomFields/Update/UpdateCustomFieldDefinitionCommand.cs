using System;
using Aseta.Application.Abstractions.Authorization;
using Aseta.Application.Abstractions.Messaging;
using Aseta.Domain.Entities.Inventories.CustomField;
using Aseta.Domain.Entities.InventoryRoles;

namespace Aseta.Application.CustomFields.Update;

[Authorize(Role.Owner)]
public sealed record UpdateCustomFieldDefinitionCommand(
    Guid InventoryId,
    Guid FieldId,
    string Name,
    CustomFieldType Type) : ICommand, IInventoryScopedRequest;
