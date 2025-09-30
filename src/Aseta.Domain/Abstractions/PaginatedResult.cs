namespace Aseta.Domain.Abstractions;

public record PaginatedResult<T>(
    List<T> Items,
    int PageNumber,
    int PageSize,
    int TotalCount,
    bool HasNextPage
);