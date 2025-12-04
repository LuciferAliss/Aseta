using Aseta.Domain.Abstractions.Primitives.Pagination;

namespace Aseta.Application.Inventories.GetPaginated.Contracts;

public sealed record InventoriesResponse(
    KeysetPage<InventoryResponse> Inventories
);
