namespace Aseta.Domain.DTO.Inventories;

public record class InventoryPaginationParameters(
    DateTime? CreatedAtFrom,
    DateTime? CreatedAtTo,
    ICollection<Guid> TagIds,
    ICollection<Guid> CategoryIds,
    int? MinItemsCount,
    int? MaxItemsCount,
    SortBy SortBy,
    string SortOrder,
    string? Cursor,
    int PageSize
);

public enum SortBy
{
    None,
    Date,
    Name,
    Creator,
    NumberOfItems
}