using MicroServices.Web.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MicroServices.Web.Services.IServices;

public interface IProductService
{
    Task<IEnumerable<ProductViewModel>> FindAllProductsAsync(string token);
    Task<ProductViewModel> FindProductByIdAsync(long id, string token);
    Task<ProductViewModel> CreateProductAsync(ProductViewModel model, string token);
    Task<ProductViewModel> UpdateProductAsync(ProductViewModel model, string token);
    Task<bool> DeleteProductByIdAsync(long id, string token);
}
