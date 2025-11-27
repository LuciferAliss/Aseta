using Aseta.Domain.Abstractions.Primitives.Pagination;
using Aseta.Domain.Entities.Inventories;

namespace Aseta.Application.Inventories.GetPaginated;

internal sealed record InventoryResponse(
    KeysetPage<Inventory> Inventories
);