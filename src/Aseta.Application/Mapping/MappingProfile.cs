using Aseta.Domain.DTO.Category;
using Aseta.Domain.DTO.CustomField;
using Aseta.Domain.DTO.CustomId;
using Aseta.Domain.DTO.Inventory;
using Aseta.Domain.DTO.Item;
using Aseta.Domain.DTO.Tag;
using Aseta.Domain.DTO.User;
using Aseta.Domain.Entities.CustomField;
using Aseta.Domain.Entities.CustomId;
using Aseta.Domain.Entities.Inventories;
using Aseta.Domain.Entities.Items;
using Aseta.Domain.Entities.Tags;
using Aseta.Domain.Entities.Users;
using AutoMapper;

namespace Aseta.Application.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<UserApplication, UserAdminViewResponse>()
            .ForMember(
                dest => dest.IsAdmin,
                opt => opt.MapFrom(src =>
                    src.InventoryUserRoles.Any(r => r.Role.ToString() == "Admin")
                )
            )
            .ForMember(
                dest => dest.IsBlocked,
                opt => opt.MapFrom(src =>
                    src.LockoutEnd.HasValue && src.LockoutEnd.Value > DateTimeOffset.UtcNow
                )
            );

        CreateMap<Inventory, ViewInventoryResponse>()
            .ForMember(dest => dest.Creator, opt =>
                opt.MapFrom(src => new UserInventoryInfoResponse(src.Creator.Id, src.Creator.UserName)));

        CreateMap<Inventory, InventoryResponse>()
            .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags))
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category))
            .ForMember(dest => dest.UserCreator, opt =>
                opt.MapFrom(src => new UserInventoryInfoResponse(src.Creator.Id, src.Creator.UserName)))
            .ForMember(dest => dest.CustomFieldsDefinition, opt => opt.MapFrom(src => src.CustomFields))
            .ForMember(dest => dest.CustomIdRules, opt => opt.MapFrom(src => src.CustomIdRules));

        CreateMap<Item, ItemResponse>()
            .ForMember(dest => dest.CustomFields, opt => opt.MapFrom(src => src.CustomFieldValues))
            .ForMember(dest => dest.UserCreate, opt =>
                opt.MapFrom(src => new UserInventoryInfoResponse(src.Creator.Id, src.Creator.UserName)))
            .ForMember(dest => dest.UserUpdate, opt =>
                opt.MapFrom(src => new UserInventoryInfoResponse(src.Updater.Id, src.Updater.UserName)));

        CreateMap<Category, CategoryResponse>();
        CreateMap<Tag, TagResponse>()
            .ForMember(dest => dest.Weight, opt => opt.MapFrom(src => src.Inventories.Count));

        CreateMap<CustomFieldValue, CustomFieldValueResponse>();

        CreateMap<CustomFieldDefinition, CustomFieldValue>()
            .ForMember(dest => dest.FieldId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Value, opt => opt.MapFrom<CustomFieldValueResolver>());

        CreateMap<CustomFieldDefinition, CustomFieldDefinitionResponse>()
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => (int)src.Type));

        CreateMap<CustomIdRuleBase, CustomIdRulePartResponse>()
            .Include<DateRule, DateRuleResponse>()
            .Include<GuidRule, GuidRuleResponse>()
            .Include<FixedTextRule, FixedTextRuleResponse>()
            .Include<RandomDigitsRule, RandomDigitsRuleResponse>()
            .Include<RandomNumberBitRule, RandomNumberBitRuleResponse>()
            .Include<SequenceRule, SequenceRuleResponse>();

        CreateMap<DateRule, DateRuleResponse>()
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => "date"));

        CreateMap<GuidRule, GuidRuleResponse>()
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => "guid"));

        CreateMap<FixedTextRule, FixedTextRuleResponse>()
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => "fixed_text"));

        CreateMap<RandomDigitsRule, RandomDigitsRuleResponse>()
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => "random_digits"));

        CreateMap<RandomNumberBitRule, RandomNumberBitRuleResponse>()
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => "random_bits"));

        CreateMap<SequenceRule, SequenceRuleResponse>()
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => "sequence"));

    }
}