namespace Aseta.Application.Items.GetPaginated.Contracts;

public sealed record ItemResponse(
    Guid Id,
    string CustomId,
    List<CustomFieldValueResponse> CustomFieldValues,
    string CreatorName,
    string? UpdaterName,
    DateTime CreatedAt,
    DateTime? UpdatedAt);