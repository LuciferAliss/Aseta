using Aseta.Application.Inventories.Get.Contracts;
using Aseta.Domain.Entities.Categories;
using Aseta.Domain.Entities.CustomField;
using Aseta.Domain.Entities.Inventories;
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
    }
}
