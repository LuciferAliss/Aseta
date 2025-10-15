namespace Aseta.Application.Items.GetView;

public sealed record CustomFieldValueResponse(
    Guid FieldId,
    string? Value
);