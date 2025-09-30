using Aseta.Domain.Abstractions;
using Aseta.Domain.Abstractions.Messaging;

namespace Aseta.Application.Items.GetView;

public sealed record GetViewItemQuery(
    Guid InventoryId,
    int PageNumber,
    int PageSize
) : IQuery<PaginatedResult<ItemViewResponse>>;