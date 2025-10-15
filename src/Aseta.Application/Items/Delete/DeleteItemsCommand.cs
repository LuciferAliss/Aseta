using Aseta.Application.Abstractions.Messaging;

namespace Aseta.Application.Items.Delete;

public sealed record DeleteItemsCommand(
    ICollection<Guid> ItemIds,
    Guid InventoryId) : ICommand;
