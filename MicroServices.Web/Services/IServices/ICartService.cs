using MicroServices.Web.Models;
using System.Threading.Tasks;

namespace MicroServices.Web.Services.IServices;

public interface ICartService
{
    Task<CartViewModel> FindCartByUserIdAsync(string userId, string token);
    Task<CartViewModel> AddItemToCartAsync(CartViewModel cart, string token);
    Task<CartViewModel> UpdateCartAsync(CartViewModel cart, string token);
    Task<bool> RemoveFormCartAsync(long cartId, string token);
    Task<bool> ApplyCouponAsync(CartViewModel cart, string token);
    Task<bool> RemoveCouponAsync(string userId, string token);
    Task<bool> ClearCartAsync(string userId, string token);
    Task<CartViewModel> CheckoutAsync(CartHeaderViewModel cart, string token);
}
