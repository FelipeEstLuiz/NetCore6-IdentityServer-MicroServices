using MicroServices.CartAPI.Data.ValueObjects;

namespace MicroServices.CartAPI.Repository;

public interface ICartRepository
{
    Task<CartVO> FindCartByUserIdAsync(string userId);
    Task<CartVO> SaveOrUpdateCartAsync(CartVO cart);
    Task<bool> RemoveFromCartAsync(long cartDetailsId);
    Task<bool> ApplyCouponAsync(string userId, string couponCode);
    Task<bool> RemoveCouponAsync(string userId);
    Task<bool> ClearCartAsync(string userId);
}