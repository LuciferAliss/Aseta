using Aseta.Domain.Abstractions.Primitives.Pagination;

namespace Aseta.Application.Items.GetPaginated.Contracts;

public sealed record ItemsResponse
(
    KeysetPage<ItemResponse> Items
);
