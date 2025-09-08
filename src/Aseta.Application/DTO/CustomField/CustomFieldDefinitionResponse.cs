namespace Aseta.Application.DTO.CustomField;

public record CustomFieldDefinitionResponse
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public string Type { get; init; }
    public bool ShowInTableView { get; init; }
}