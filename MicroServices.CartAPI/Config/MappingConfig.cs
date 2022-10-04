using AutoMapper;
using MicroServices.CartAPI.Data.ValueObjects;
using MicroServices.CartAPI.Model;

namespace MicroServices.CartAPI.Config;

public class MappingConfig
{
    public static MapperConfiguration RegisterMaps()
    {
        MapperConfiguration mappingConfig = new(config => {
            config.CreateMap<ProductVO, Product>().ReverseMap();
            config.CreateMap<CartHeaderVO, CartHeader>().ReverseMap();
            config.CreateMap<CartDetailVO, CartDetail>().ReverseMap();
            config.CreateMap<CartVO, Cart>().ReverseMap();
        });
        return mappingConfig;
    }
}
