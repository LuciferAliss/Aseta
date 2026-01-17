using Aseta.Application.Abstractions.Messaging;

namespace Aseta.Application.Tags.AddTagsInventory;

public sealed record AddTagsInventoryCommand(Guid InventoryId, Guid[] TagIds) : ICommand;
