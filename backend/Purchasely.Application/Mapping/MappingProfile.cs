using AutoMapper;
using Purchasely.Application.DTOs;
using Purchasely.Domain.Entities;

namespace Purchasely.Application.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Supplier, SupplierResponse>();
    }
}