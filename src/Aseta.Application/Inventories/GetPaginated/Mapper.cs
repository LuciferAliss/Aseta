using Aseta.Application.Inventories.GetPaginated.Contracts;
using Aseta.Domain.DTO.Inventories;
using Aseta.Domain.Entities.Inventories;
using AutoMapper;

namespace Aseta.Application.Inventories.GetPaginated;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<GetPaginatedInventoryQuery, InventoryPaginationParameters>();

        CreateMap<Inventory, InventoryResponse>()
            .ForMember(
                dest => dest.Name,
                opt => opt.MapFrom(src => src.InventoryName))
            .ForMember(
                dest => dest.CreatorName,
                opt => opt.MapFrom(src => src.Creator.UserName));
    }
}