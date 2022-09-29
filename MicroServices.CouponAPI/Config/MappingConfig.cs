using AutoMapper;

namespace MicroServices.CouponAPI.Config;
public class MappingConfig
{
    public static MapperConfiguration RegisterMaps()
    {
        MapperConfiguration mappingConfig = new MapperConfiguration(config =>
        {
        });
        return mappingConfig;
    }
}