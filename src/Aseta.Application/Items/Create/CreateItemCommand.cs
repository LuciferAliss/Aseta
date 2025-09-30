using Aseta.Domain.Abstractions.Messaging;
using Aseta.Domain.Entities.CustomField;

namespace Aseta.Application.Items.Create;

public sealed record CreateItemCommand(
    ICollection<CustomFieldValue> CustomFieldsValue,
    Guid InventoryId,
    Guid UserId
) : ICommand;