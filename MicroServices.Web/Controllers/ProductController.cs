using MicroServices.Web.Models;
using MicroServices.Web.Services.IServices;
using MicroServices.Web.Utils;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MicroServices.Web.Controllers;

public class ProductController : Controller
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService ?? throw new ArgumentNullException(nameof(productService));
    }

    [Authorize]
    public async Task<IActionResult> ProductIndex()
    {
        string token = await HttpContext.GetTokenAsync("access_token");
        IEnumerable<ProductViewModel> products = await _productService.FindAllProducts(token);

        return View(products);
    }

    public IActionResult ProductCreate()
    {
        return View();
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> ProductCreate(ProductViewModel productModel)
    {
        if (ModelState.IsValid)
        {
            string token = await HttpContext.GetTokenAsync("access_token");
            ProductViewModel response = await _productService.CreateProduct(productModel, token);

            if (response != null)
                return RedirectToAction(nameof(ProductIndex));
        }

        return View(productModel);
    }

    public async Task<IActionResult> ProductUpdate(long id)
    {
        string token = await HttpContext.GetTokenAsync("access_token");
        ProductViewModel product = await _productService.FindProductById(id, token);

        if (product != null)
            return View(product);

        return NotFound();
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> ProductUpdate(ProductViewModel productModel)
    {
        if (ModelState.IsValid)
        {
            string token = await HttpContext.GetTokenAsync("access_token");
            ProductViewModel response = await _productService.UpdateProduct(productModel, token);

            if (response != null)
                return RedirectToAction(nameof(ProductIndex));
        }

        return View(productModel);
    }


    public async Task<IActionResult> ProductDelete(long id)
    {
        string token = await HttpContext.GetTokenAsync("access_token");
        ProductViewModel model = await _productService.FindProductById(id, token);

        if (model != null)
            return View(model);

        return NotFound();
    }

    [HttpPost]
    [Authorize(Roles = Role.Admin)]
    public async Task<IActionResult> ProductDelete(ProductViewModel productModel)
    {
        string token = await HttpContext.GetTokenAsync("access_token");
        bool response = await _productService.DeleteProductById(productModel.Id, token);

        if (response)
            return RedirectToAction(nameof(ProductIndex));

        return View(productModel);
    }
}
