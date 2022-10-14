namespace MicroServices.CartAPI.Repository;

public interface ICouponRepository
{
    Task<CouponVO> GetCouponAsync(string couponCode, string token);
}