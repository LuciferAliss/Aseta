using Aseta.Application.Abstractions.Authorization;
using Aseta.Application.Abstractions.Messaging;

namespace Aseta.Application.Inventories.Create;

[Authorize]
public sealed record CreateInventoryCommand(
    string Name,
    string Description,
    Uri? ImageUrl,
    bool IsPublic,
    Guid CategoryId,
    Guid UserId) : ICommand<InventoryResponse>;