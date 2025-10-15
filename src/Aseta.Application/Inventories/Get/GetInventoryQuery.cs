using Aseta.Application.Abstractions.Messaging;

namespace Aseta.Application.Inventories.Get;

public sealed record GetInventoryQuery(Guid InventoryId) : IQuery<InventoryResponse>;
