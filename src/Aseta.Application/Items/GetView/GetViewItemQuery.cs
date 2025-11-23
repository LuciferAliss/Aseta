using Aseta.Application.Abstractions.Messaging;
using Aseta.Domain.Abstractions.Primitives;

namespace Aseta.Application.Items.GetView;

public sealed record GetViewItemQuery(
    Guid InventoryId,
    int PageNumber,
    int PageSize) : IQuery<PaginatedResponse<ItemResponse>>;