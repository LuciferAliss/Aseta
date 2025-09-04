using System;
using Aseta.Application.Services;
using Aseta.Domain.Entities.Inventories;
using AutoMapper;

namespace Aseta.Application.Mapping;

public class MappingProfile : Profile
{    
    public MappingProfile()
    {
        CreateMap<Inventory, InventoryResponse>();
    }
}