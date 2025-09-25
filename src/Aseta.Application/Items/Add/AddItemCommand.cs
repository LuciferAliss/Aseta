using Aseta.Domain.Abstractions.Messaging;
using Aseta.Domain.DTO.CustomField;

namespace Aseta.Application.Items.Add;

public sealed record AddItemCommand(
    List<CustomFieldValueRequest> CustomFields,
    Guid InventoryId,
    Guid UserId
) : ICommand;