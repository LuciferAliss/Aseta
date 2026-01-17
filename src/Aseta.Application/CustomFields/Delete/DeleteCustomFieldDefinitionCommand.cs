using System;
using Aseta.Application.Abstractions.Authorization;
using Aseta.Application.Abstractions.Messaging;
using Aseta.Domain.Entities.InventoryRoles;

namespace Aseta.Application.CustomFields.Delete;

[Authorize(Role.Owner)]
public sealed record DeleteCustomFieldDefinitionCommand(
    Guid FieldId,
    Guid InventoryId) : ICommand, IInventoryScopedRequest;
