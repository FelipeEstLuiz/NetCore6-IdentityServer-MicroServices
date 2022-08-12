using AutoMapper;
using MicroServices.ProductAPI.Data.ValueObjects;
using MicroServices.ProductAPI.Model;

namespace MicroServices.ProductAPI.Config
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            MapperConfiguration mappingConfig = new MapperConfiguration(config => {
                config.CreateMap<ProductVO, Product>().ReverseMap();
            });
            return mappingConfig;
        }
    }
}
