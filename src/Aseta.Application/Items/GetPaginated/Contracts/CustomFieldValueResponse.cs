namespace Aseta.Application.Items.GetPaginated.Contracts;

public sealed record CustomFieldValueResponse(
    Guid FieldId,
    string? Value);