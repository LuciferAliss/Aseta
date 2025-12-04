namespace Aseta.Application.Items.Create;

public sealed record CustomFieldValueRequest(Guid FieldId, string? Value);