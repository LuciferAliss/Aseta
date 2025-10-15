using Aseta.Application.Abstractions.Messaging;

namespace Aseta.Application.Inventories.Delete;

public sealed record DeleteInventoryCommand(Guid InventoryId) : ICommand;
