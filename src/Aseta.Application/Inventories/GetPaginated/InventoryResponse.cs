using Aseta.Domain.Abstractions.Primitives;
using Aseta.Domain.Entities.Inventories;

namespace Aseta.Application.Inventories.GetPaginated;

internal sealed record InventoryResponse(
    KeysetPage<Inventory> Inventories
);