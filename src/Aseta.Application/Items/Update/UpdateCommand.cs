using Aseta.Application.Abstractions.Messaging;
using Aseta.Domain.Entities.CustomField;

namespace Aseta.Application.Items.Update;

public sealed record UpdateCommand(
    Guid ItemId,
    string CustomId,
    ICollection<CustomFieldValue> CustomFieldsValue,
    Guid InventoryId,
    Guid UserId) : ICommand;