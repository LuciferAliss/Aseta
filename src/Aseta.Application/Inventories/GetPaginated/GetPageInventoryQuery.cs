using Aseta.Application.Abstractions.Messaging;
using Aseta.Domain.DTO.Inventory;

namespace Aseta.Application.Inventories.GetPaginated;

internal sealed record GetPaginatedInventoryQuery(
    DateTime? CreatedAtFrom,
    DateTime? CreatedAtTo,
    ICollection<Guid> TagIds,
    ICollection<Guid> CategoryIds,
    bool? IsPublic,
    int? MinItemsCount,
    int? MaxItemsCount,
    SortBy SortBy,
    string SortOrder,
    string? Cursor,
    int PageSize
) : IQuery<InventoryResponse>;
