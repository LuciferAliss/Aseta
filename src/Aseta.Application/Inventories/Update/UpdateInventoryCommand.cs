using Aseta.Application.Abstractions.Messaging;

namespace Aseta.Application.Inventories.Update;

public sealed record UpdateInventoryCommand(
    Guid InventoryId,
    string Name,
    string Description,
    string ImageUrl,
    bool IsPublic) : ICommand;