using Aseta.Domain.DTO.Inventory;
using AutoMapper;

namespace Aseta.Application.Inventories.GetPaginated;

internal sealed class Mapper : Profile
{
    public Mapper()
    {
        CreateMap<GetPaginatedInventoryQuery, InventoryPaginationParameters>();
    }
}
