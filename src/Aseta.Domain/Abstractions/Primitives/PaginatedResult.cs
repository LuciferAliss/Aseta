namespace Aseta.Domain.Abstractions.Primitives;

public sealed record PaginatedResult<T>(
    ICollection<T> Values,
    int PageNumber,
    int PageSize,
    int TotalCount,
    bool HasNextPage)
{
    public static PaginatedResult<T> Create(ICollection<T> values, int pageNumber, int pageSize, int totalCount)
    {
        return new PaginatedResult<T>(
            values,
            pageNumber,
            pageSize,
            totalCount,
            pageNumber * pageSize < totalCount
        );
    }
}