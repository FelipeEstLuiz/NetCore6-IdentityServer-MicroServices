using System.Linq;
using System.Threading.Tasks;
using MicroServices.Web.Models;
using MicroServices.Web.Services.IServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MicroServices.Web.Controllers;

public class CartController : Controller
{
    private readonly IProductService _productService;
    private readonly ICartService _cartService;

    public CartController(
        IProductService productService,
        ICartService cartService
    )
    {
        _productService = productService;
        _cartService = cartService;
    }

    [Authorize]
    public async Task<IActionResult> CartIndex()
    {
        return View(await FindUserCart());
    }

    public async Task<IActionResult> Remove(int id)
    {
        string token = await HttpContext.GetTokenAsync("access_token");
        string userId = User.Claims.Where(u => u.Type == "sub")?.FirstOrDefault()?.Value;

        bool response = await _cartService.RemoveFormCartAsync(id, token);

        if (response)
            return RedirectToAction(nameof(CartIndex));

        return View();
    }

    private async Task<CartViewModel> FindUserCart()
    {
        string token = await HttpContext.GetTokenAsync("access_token");
        string userId = User.Claims.Where(u => u.Type == "sub")?.FirstOrDefault()?.Value;

        CartViewModel response = await _cartService.FindCartByUserIdAsync(userId, token);

        if (response?.CartHeader != null)
        {
            foreach (CartDetailViewModel detail in response.CartDetails)
                response.CartHeader.PurchaseAmount += detail.Product.Price * detail.Count;
        }

        return response;
    }
}