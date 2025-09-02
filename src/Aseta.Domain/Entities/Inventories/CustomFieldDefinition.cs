using System;

namespace Aseta.Domain.Entities.Inventories;

public class CustomFieldDefinition
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public CustomFieldType Type { get; set; }
    public bool ShowInTableView { get; set; }
    
    private CustomFieldDefinition() { }

    private CustomFieldDefinition(Guid id, string name, string description, CustomFieldType type, bool showInTableView)
    {
        Id = id;
        Name = name;
        Description = description;
        Type = type;
        ShowInTableView = showInTableView;
    }   

    public static CustomFieldDefinition Create(string name, string description, CustomFieldType type, bool showInTableView)
    {
        return new CustomFieldDefinition
        (
            Guid.NewGuid(),
            name,
            description,
            type,
            showInTableView
        );
    }
}