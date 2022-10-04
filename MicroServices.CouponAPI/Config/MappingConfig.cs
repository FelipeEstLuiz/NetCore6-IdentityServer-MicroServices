using AutoMapper;
using MicroServices.CouponAPI.Data.ValueObjects;
using MicroServices.CouponAPI.Model;

namespace MicroServices.CouponAPI.Config;
public class MappingConfig
{
    public static MapperConfiguration RegisterMaps()
    {
        MapperConfiguration mappingConfig = new(config =>
        {
            config.CreateMap<CouponVO, Coupon>().ReverseMap();
        });
        return mappingConfig;
    }
}