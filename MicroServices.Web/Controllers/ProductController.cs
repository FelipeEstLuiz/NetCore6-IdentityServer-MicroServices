using MicroServices.Web.Models;
using MicroServices.Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroServices.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService ?? throw new ArgumentNullException(nameof(productService));
        }

        public async Task<IActionResult> ProductIndex()
        {
            IEnumerable<ProductModel> products = await _productService.FindAllProducts();

            return View(products);
        }

        public IActionResult ProductCreate()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ProductCreate(ProductModel productModel)
        {
            if (ModelState.IsValid)
            {
                ProductModel response = await _productService.CreateProduct(productModel);

                if (response != null)
                    return RedirectToAction(nameof(ProductIndex));
            }

            return View(productModel);
        }

        public async Task<IActionResult> ProductUpdate(long id)
        {
            ProductModel product = await _productService.FindProductById(id);

            if (product != null)
                return View(product);

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> ProductUpdate(ProductModel productModel)
        {
            if (ModelState.IsValid)
            {
                ProductModel response = await _productService.UpdateProduct(productModel);

                if (response != null)
                    return RedirectToAction(nameof(ProductIndex));
            }

            return View(productModel);
        }

        public async Task<IActionResult> ProductDelete(long id)
        {
            ProductModel model = await _productService.FindProductById(id);

            if (model != null)
                return View(model);

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> ProductDelete(ProductModel productModel)
        {
            bool response = await _productService.DeleteProductById(productModel.Id);

            if (response)
                return RedirectToAction(nameof(ProductIndex));

            return View(productModel);
        }
    }
}
