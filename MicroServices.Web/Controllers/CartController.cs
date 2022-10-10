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
    private readonly ICouponService _couponService;

    public CartController(
        IProductService productService,
        ICartService cartService,
        ICouponService couponService
    )
    {
        _productService = productService;
        _cartService = cartService;
        _couponService = couponService;
    }

    [Authorize]
    public async Task<IActionResult> CartIndex()
    {
        return View(await FindUserCart());
    }

    [HttpPost]
    [ActionName("ApplyCoupon")]
    public async Task<IActionResult> ApplyCoupon(CartViewModel model)
    {
        string token = await HttpContext.GetTokenAsync("access_token");
        string userId = User.Claims.Where(u => u.Type == "sub")?.FirstOrDefault()?.Value;

        bool response = await _cartService.ApplyCouponAsync(model, token);

        if (response)
            return RedirectToAction(nameof(CartIndex));

        return View();
    }

    [HttpPost]
    [ActionName("RemoveCoupon")]
    public async Task<IActionResult> RemoveCoupon()
    {
        string token = await HttpContext.GetTokenAsync("access_token");
        string userId = User.Claims.Where(u => u.Type == "sub")?.FirstOrDefault()?.Value;

        bool response = await _cartService.RemoveCouponAsync(userId, token);

        if (response)
            return RedirectToAction(nameof(CartIndex));

        return View();
    }

    public async Task<IActionResult> Remove(int id)
    {
        string token = await HttpContext.GetTokenAsync("access_token");
        string userId = User.Claims.Where(u => u.Type == "sub")?.FirstOrDefault()?.Value;

        bool response = await _cartService.RemoveFromCartAsync(id, token);

        if (response)
            return RedirectToAction(nameof(CartIndex));

        return View();
    }

    [HttpGet]
    public async Task<IActionResult> Checkout()
    {
        return View(await FindUserCart());
    }

    [HttpPost]
    public async Task<IActionResult> Checkout(CartViewModel model)
    {
        string token = await HttpContext.GetTokenAsync("access_token");
        CartHeaderViewModel response = await _cartService.CheckoutAsync(model.CartHeader, token);

        if (response is not null)
            return RedirectToAction(nameof(Confirmation));

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Confirmation(CartViewModel model)
    {
        return View();
    }

    private async Task<CartViewModel> FindUserCart()
    {
        string token = await HttpContext.GetTokenAsync("access_token");
        string userId = User.Claims.Where(u => u.Type == "sub")?.FirstOrDefault()?.Value;

        CartViewModel response = await _cartService.FindCartByUserIdAsync(userId, token);

        if (response?.CartHeader != null)
        {
            if (!string.IsNullOrEmpty(response.CartHeader.CouponCode))
            {
                CouponViewModel coupon = await _couponService.GetCouponAsync(response.CartHeader.CouponCode, token);

                if (coupon?.CouponCode != null)
                    response.CartHeader.DiscountAmount = coupon.DiscountAmount;
            }
            foreach (CartDetailViewModel detail in response.CartDetails)
                response.CartHeader.PurchaseAmount += (detail.Product.Price * detail.Count);

            response.CartHeader.PurchaseAmount -= response.CartHeader.DiscountAmount;
        }
        return response;
    }
}