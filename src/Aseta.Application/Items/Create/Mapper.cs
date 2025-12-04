using System;
using Aseta.Domain.Entities.Inventories.CustomField;
using AutoMapper;

namespace Aseta.Application.Items.Create;

internal sealed class Mapper : Profile
{
    public Mapper()
    {
        CreateMap<CustomFieldValueRequest, CustomFieldValue>();
    }
}
