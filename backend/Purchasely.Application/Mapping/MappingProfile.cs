using AutoMapper;
using Purchasely.Application.DTOs;
using Purchasely.Domain.Entities;

namespace Purchasely.Application.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Supplier, SupplierListResponse>();
        CreateMap<Supplier, SupplierDetailsResponse>()
            .ForMember(
                dest => dest.Products,
                opt => opt.MapFrom(src => src.Products)
            );
        CreateMap<Product, ProductResponse>();
        CreateMap<Product, SupplierProducts>();
        CreateMap<User, UserResponse>();
    }
}