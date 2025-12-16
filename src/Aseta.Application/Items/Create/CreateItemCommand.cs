using Aseta.Application.Abstractions.Authorization;
using Aseta.Application.Abstractions.Messaging;
using Aseta.Domain.Entities.InventoryRoles;

namespace Aseta.Application.Items.Create;

[Authorize(Role.Editor)]
public sealed record CreateItemCommand(
    ICollection<CustomFieldValueData> CustomFieldValues,
    Guid InventoryId,
    Guid UserId) : ICommand, IInventoryScopedRequest;
