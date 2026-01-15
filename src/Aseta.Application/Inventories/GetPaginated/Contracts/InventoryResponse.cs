using Aseta.Domain.Entities.Inventories;

namespace Aseta.Application.Inventories.GetPaginated.Contracts;

public sealed record InventoryResponse(
    Guid Id,
    string Name,
    Uri ImageUrl,
    int ItemsCount,
    string CreatorName,
    CategoryResponse Category,
    DateTime CreatedAt);