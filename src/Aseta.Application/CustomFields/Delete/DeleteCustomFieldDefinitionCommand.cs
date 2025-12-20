using System;
using Aseta.Application.Abstractions.Authorization;
using Aseta.Application.Abstractions.Messaging;
using Aseta.Domain.Entities.InventoryRoles;

namespace Aseta.Application.CustomFields.Delete;

[Authorize(Role.Owner)]
public sealed record DeleteCustomFieldDefinitionCommand(
    ICollection<CustomFieldData> CustomFields,
    Guid InventoryId) : ICommand, IInventoryScopedRequest;
