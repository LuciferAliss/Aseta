namespace Aseta.Application.Items.GetPaginated.Contracts;

public sealed record ItemResponse(
    Guid Id,
    string CustomId,
    List<CustomFieldValueResponse> CustomFields,
    string UpdaterName,
    string CreatorName,
    DateTime CreatedAt,
    DateTime UpdatedAt);