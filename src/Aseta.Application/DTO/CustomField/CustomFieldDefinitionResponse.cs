namespace Aseta.Application.DTO.CustomField;

public record CustomFieldDefinitionResponse
(
    Guid Id, 
    string Name,
    string Type,
    bool ShowInTableView
);