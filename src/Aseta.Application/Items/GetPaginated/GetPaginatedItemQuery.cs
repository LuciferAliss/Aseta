using Aseta.Application.Abstractions.Messaging;
using Aseta.Application.Items.GetPaginated.Contracts;
using Aseta.Domain.Abstractions.Primitives;
using Aseta.Domain.Abstractions.Primitives.Pagination;
using Aseta.Domain.DTO.Items;

namespace Aseta.Application.Items.GetPaginated;

public sealed record GetPaginatedItemQuery(
    Guid InventoryId,
    DateTime? CreatedAtFrom,
    DateTime? CreatedAtTo,
    DateTime? UpdatedAtFrom,
    DateTime? UpdatedAtTo,
    Guid? CreatorId,
    Guid? UpdaterId,
    SortBy SortBy,
    string SortOrder,
    string? Cursor,
    int PageSize) : IQuery<ItemsResponse>;