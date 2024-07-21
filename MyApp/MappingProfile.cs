using AutoMapper;
using MyApp.Data.Entities;
using MyApp.DTOs;

namespace MyApp
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Client, ClientDto>()
                .ForMember(dest => dest.Sales, opt => opt.MapFrom(src => src.Sales));
            CreateMap<Sales, SalesDto>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.Product.Id))
                .ForMember(dest => dest.ClientId, opt => opt.MapFrom(src => src.Client.Id));
            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));
        }
    }
}
