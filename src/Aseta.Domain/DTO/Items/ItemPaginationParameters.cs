namespace Aseta.Domain.DTO.Items;

public record ItemPaginationParameters(
    DateTime? CreatedAtFrom,
    DateTime? CreatedAtTo,
    DateTime? UpdatedAtFrom,
    DateTime? UpdatedAtTo,
    Guid? CreatorId,
    Guid? UpdaterId,
    SortBy SortBy,
    string SortOrder,
    string? Cursor,
    int PageSize);
