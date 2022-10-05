using MicroServices.Web.Models;
using MicroServices.Web.Services.IServices;
using MicroServices.Web.Utils;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace MicroServices.Web.Services;

public class CartService : ICartService
{
    private readonly HttpClient _client;
    public const string BasePath = "api/v1/cart";

    public CartService(HttpClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async Task<CartViewModel> FindCartByUserIdAsync(string userId, string token)
    {
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        HttpResponseMessage response = await _client.GetAsync($"{BasePath}/find-cart/{userId}");
        return await response.ReadContentAs<CartViewModel>();
    }

    public async Task<CartViewModel> AddItemToCartAsync(CartViewModel model, string token)
    {
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        HttpResponseMessage response = await _client.PostAsJson($"{BasePath}/add-cart", model);
        if (response.IsSuccessStatusCode)
            return await response.ReadContentAs<CartViewModel>();
        throw new Exception("Something went wrong when calling API");
    }

    public async Task<CartViewModel> UpdateCartAsync(CartViewModel model, string token)
    {
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        HttpResponseMessage response = await _client.PutAsJson($"{BasePath}/update-cart", model);
        if (response.IsSuccessStatusCode)
            return await response.ReadContentAs<CartViewModel>();
        throw new Exception("Something went wrong when calling API");
    }

    public async Task<bool> RemoveFromCartAsync(long cartId, string token)
    {
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        HttpResponseMessage response = await _client.DeleteAsync($"{BasePath}/remove-cart/{cartId}");
        if (response.IsSuccessStatusCode)
            return await response.ReadContentAs<bool>();
        throw new Exception("Something went wrong when calling API");
    }

    public async Task<bool> ApplyCouponAsync(CartViewModel model, string token)
    {
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        HttpResponseMessage response = await _client.PostAsJson($"{BasePath}/apply-coupon", model);
        if (response.IsSuccessStatusCode)
            return await response.ReadContentAs<bool>();
        throw new Exception("Something went wrong when calling API");
    }

    public async Task<bool> RemoveCouponAsync(string userId, string token)
    {
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        HttpResponseMessage response = await _client.DeleteAsync($"{BasePath}/remove-coupon/{userId}");
        if (response.IsSuccessStatusCode)
            return await response.ReadContentAs<bool>();
        throw new Exception("Something went wrong when calling API");
    }

    public async Task<CartViewModel> CheckoutAsync(CartHeaderViewModel cartHeader, string token)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> ClearCartAsync(string userId, string token)
    {
        throw new NotImplementedException();
    }
}