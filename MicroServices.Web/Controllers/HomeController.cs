using System.Linq;
using MicroServices.Web.Models;
using MicroServices.Web.Services.IServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace MicroServices.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IProductService _productService;
    private readonly ICartService _cartService;

    public HomeController(
        ILogger<HomeController> logger,
        IProductService productService,
        ICartService cartService
    )
    {
        _logger = logger;
        _productService = productService;
        _cartService = cartService;
    }

    public async Task<IActionResult> Index()
    {
        IEnumerable<ProductViewModel> products = await _productService.FindAllProducts(string.Empty);

        return View(products);
    }

    [Authorize]
    public async Task<IActionResult> Details(int id)
    {
        string token = await HttpContext.GetTokenAsync("access_token");
        ProductViewModel product = await _productService.FindProductById(id, token);

        return View(product);
    }

    [HttpPost]
    [ActionName("Details")]
    [Authorize]
    public async Task<IActionResult> DetailsPost(ProductViewModel model)
    {
        string token = await HttpContext.GetTokenAsync("access_token");

        CartViewModel cart = new()
        {
            CartHeader = new()
            {
                UserId = User.Claims.Where(u => u.Type == "sub")?.FirstOrDefault()?.Value
            }
        };

        CartDetailViewModel cartDetail = new()
        {
            Count = model.Count,
            ProductId = model.Id,
            Product = await _productService.FindProductById(model.Id, token)
        };

        cart.CartDetails = new List<CartDetailViewModel>() { cartDetail };

        CartViewModel response = await _cartService.AddItemToCartAsync(cart, token);

        if (response is not null)
            return RedirectToAction(nameof(Index));

        return View(model);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    [Authorize]
    public async Task<IActionResult> Login()
    {
        var accessToken = await HttpContext.GetTokenAsync("access_token");
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Logout()
    {
        return SignOut("Cookies", "oidc");
    }
}
