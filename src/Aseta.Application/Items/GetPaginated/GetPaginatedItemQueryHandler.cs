using Aseta.Application.Abstractions.Messaging;
using Aseta.Application.Items.GetPaginated.Contracts;
using Aseta.Domain.Abstractions.Persistence;
using Aseta.Domain.Abstractions.Primitives.Pagination;
using Aseta.Domain.Abstractions.Primitives.Results;
using Aseta.Domain.DTO.Items;
using Aseta.Domain.Entities.Inventories;
using Aseta.Domain.Entities.Items;
using AutoMapper;

namespace Aseta.Application.Items.GetPaginated;

internal sealed class GetPaginatedItemQueryHandler(
    IItemRepository itemRepository,
    IMapper mapper) : IQueryHandler<GetPaginatedItemQuery, ItemsResponse>
{
    public async Task<Result<ItemsResponse>> Handle(
        GetPaginatedItemQuery query,
        CancellationToken cancellationToken)
    {
        ItemPaginationParameters paginationParameters = mapper.Map<ItemPaginationParameters>(query);

        (ICollection<Item>? items, string? nextCursor, bool hasNextPage) = await itemRepository.GetPaginatedWithKeysetAsync(paginationParameters, cancellationToken);

        ICollection<ItemResponse> itemResponses = mapper.Map<ICollection<ItemResponse>>(items);

        var paginationResult = new KeysetPage<ItemResponse>(nextCursor, hasNextPage, itemResponses);

        return new ItemsResponse(paginationResult);
    }
}