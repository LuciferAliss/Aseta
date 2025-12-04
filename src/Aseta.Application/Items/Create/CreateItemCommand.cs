using Aseta.Application.Abstractions.Authorization;
using Aseta.Application.Abstractions.Messaging;
using Aseta.Domain.Entities.UserRoles;

namespace Aseta.Application.Items.Create;

[Authorize(Role.Editor)]
public sealed record CreateItemCommand(
    ICollection<CustomFieldValueRequest> CustomFieldsValue,
    Guid InventoryId,
    Guid UserId) : ICommand, IInventoryScopedRequest;
