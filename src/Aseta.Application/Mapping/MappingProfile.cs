using Aseta.Application.DTO.Category;
using Aseta.Application.DTO.CustomField;
using Aseta.Application.DTO.CustomId;
using Aseta.Application.DTO.Inventory;
using Aseta.Application.DTO.Item;
using Aseta.Application.DTO.Tag;
using Aseta.Application.DTO.User;
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
            .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags))
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category))
            .ForMember(dest => dest.UserCreator, opt =>
                opt.MapFrom(src => new UserInventoryInfoResponse(src.Creator.Id, src.Creator.UserName!, src.CreatedAt)));

        CreateMap<Inventory, InventoryResponse>()
            .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags))
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category))
            .ForMember(dest => dest.UserCreator, opt =>
                opt.MapFrom(src => new UserInventoryInfoResponse(src.Creator.Id, src.Creator.UserName!, src.CreatedAt)));

        CreateMap<Item, ItemResponse>()
            .ForMember(dest => dest.CustomFields, opt => opt.MapFrom(src => src.CustomFieldValues))
            .ForMember(dest => dest.UserCreate, opt =>
                opt.MapFrom(src => new UserInventoryInfoResponse(src.Creator.Id, src.Creator.UserName!, src.CreatedAt)))
            .ForMember(dest => dest.UserUpdate, opt =>
                opt.MapFrom(src => new UserInventoryInfoResponse(src.Updater.Id, src.Updater.UserName!, src.UpdatedAt)));

        CreateMap<Category, CategoryResponse>();
        CreateMap<Tag, TagResponse>();
        CreateMap<CustomFieldValue, CustomFieldValueResponse>();
        
        CreateMap<CustomFieldDefinition, CustomFieldDefinitionResponse>()
            .ForMember(dest => dest.Type, opt => 
                opt.MapFrom(src => src.Type.ToString()));

        CreateMap<FixedTextRule, CustomIdRulePartResponse>()
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => "fixed_text"))
            .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Text));

        CreateMap<SequenceRule, CustomIdRulePartResponse>()
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => "sequence"))
            .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Padding.ToString()));

        CreateMap<DateRule, CustomIdRulePartResponse>()
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => "date"))
            .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Format));

        CreateMap<GuidRule, CustomIdRulePartResponse>()
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => "guid"))
            .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Format));
            
        CreateMap<RandomDigitsRule, CustomIdRulePartResponse>()
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => "random_digits"))
            .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Length.ToString()));

        CreateMap<RandomNumberBitRule, CustomIdRulePartResponse>()
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => "random_bits"))
            .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.CountBits.ToString()));
    }
}