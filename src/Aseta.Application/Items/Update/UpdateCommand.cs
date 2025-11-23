using Aseta.Application.Abstractions.Authorization;
using Aseta.Application.Abstractions.Messaging;
using Aseta.Domain.Entities.CustomField;
using Aseta.Domain.Entities.UserRoles;

namespace Aseta.Application.Items.Update;

[Authorize(Role.Editor)]
public sealed record UpdateCommand(
    Guid ItemId,
    string CustomId,
    ICollection<CustomFieldValue> CustomFieldsValue,
    Guid InventoryId,
    Guid UserId) : ICommand, IInventoryScopedRequest;