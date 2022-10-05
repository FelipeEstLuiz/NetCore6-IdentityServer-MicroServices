using MicroServices.Web.Models;
using MicroServices.Web.Services.IServices;
using MicroServices.Web.Utils;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace MicroServices.Web.Services;

public class ProductService : IProductService
{
    private readonly HttpClient _client;
    public const string BasePath = "api/v1/product";

    public ProductService(HttpClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async Task<IEnumerable<ProductViewModel>> FindAllProductsAsync(string token)
    {
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _client.GetAsync(BasePath);
        return await response.ReadContentAs<List<ProductViewModel>>();
    }

    public async Task<ProductViewModel> FindProductByIdAsync(long id, string token)
    {
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _client.GetAsync($"{BasePath}/{id}");
        return await response.ReadContentAs<ProductViewModel>();
    }

    public async Task<ProductViewModel> CreateProductAsync(ProductViewModel model, string token)
    {
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _client.PostAsJson(BasePath, model);
        if (response.IsSuccessStatusCode)
            return await response.ReadContentAs<ProductViewModel>();
        else throw new Exception("Something went wrong when calling API");
    }
    public async Task<ProductViewModel> UpdateProductAsync(ProductViewModel model, string token)
    {
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _client.PutAsJson(BasePath, model);
        if (response.IsSuccessStatusCode)
            return await response.ReadContentAs<ProductViewModel>();
        else throw new Exception("Something went wrong when calling API");
    }

    public async Task<bool> DeleteProductByIdAsync(long id, string token)
    {
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _client.DeleteAsync($"{BasePath}/{id}");
        if (response.IsSuccessStatusCode)
            return await response.ReadContentAs<bool>();
        else throw new Exception("Something went wrong when calling API");
    }
}
