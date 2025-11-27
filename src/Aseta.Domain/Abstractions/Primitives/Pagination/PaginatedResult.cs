namespace Aseta.Domain.Abstractions.Primitives.Pagination;
public sealed record KeysetPage<T>(
    string? Cursor,
    bool HasNextPage,
    ICollection<T> Items);