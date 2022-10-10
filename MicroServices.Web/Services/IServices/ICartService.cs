using MicroServices.Web.Models;
using System.Threading.Tasks;

namespace MicroServices.Web.Services.IServices;

public interface ICartService
{
    Task<CartViewModel> FindCartByUserIdAsync(string userId, string token);
    Task<CartViewModel> AddItemToCartAsync(CartViewModel cartViewModel, string token);
    Task<CartViewModel> UpdateCartAsync(CartViewModel cartViewModel, string token);
    Task<bool> RemoveFromCartAsync(long cartId, string token);
    Task<bool> ApplyCouponAsync(CartViewModel cartViewModel, string token);
    Task<bool> RemoveCouponAsync(string userId, string token);
    Task<bool> ClearCartAsync(string userId, string token);
    Task<CartHeaderViewModel> CheckoutAsync(CartHeaderViewModel cartHeaderViewModel, string token);
}
