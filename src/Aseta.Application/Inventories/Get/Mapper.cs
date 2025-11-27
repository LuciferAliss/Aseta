using Aseta.Application.Inventories.Get.Contracts;
using Aseta.Application.Inventories.Get.Contracts.CustomIdRule;
using Aseta.Domain.Entities.Categories;
using Aseta.Domain.Entities.Inventories;
using Aseta.Domain.Entities.Inventories.CustomField;
using Aseta.Domain.Entities.Inventories.CustomId;
using Aseta.Domain.Entities.Tags;
using AutoMapper;

namespace Aseta.Application.Inventories.Get;

internal sealed class Mapper : Profile
{
    internal Mapper()
    {
        CreateMap<Inventory, InventoryResponse>();
        CreateMap<Category, CategoryResponse>();
        CreateMap<Tag, TagResponse>();
        CreateMap<CustomFieldDefinition, CustomFieldDefinitionResponse>();

        CreateMap<CustomIdRuleBase, CustomIdRuleResponse>()
            .Include<FixedTextRule, FixedTextRuleResponse>()
            .Include<GuidRule, GuidRuleResponse>()
            .Include<SequenceRule, SequenceRuleResponse>();
            
        CreateMap<FixedTextRule, FixedTextRuleResponse>();
        CreateMap<GuidRule, GuidRuleResponse>();
        CreateMap<SequenceRule, SequenceRuleResponse>();
    }
}
