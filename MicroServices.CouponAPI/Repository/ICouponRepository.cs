using MicroServices.CouponAPI.Data.ValueObjects;

namespace MicroServices.CouponAPI.Repository;

public interface ICouponRepository
{
    Task<CouponVO> GetCouponByCouponCodeAsync(string couponCode);
}