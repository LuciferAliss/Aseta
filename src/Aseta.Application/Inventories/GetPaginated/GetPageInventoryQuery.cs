using Aseta.Application.Abstractions.Messaging;
using Aseta.Application.Inventories.GetPaginated.Contracts;
using Aseta.Domain.DTO.Inventories;

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
    int PageSize) : IQuery<InventoriesResponse>;
