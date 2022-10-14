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

    public async Task<CartViewModel> AddItemToCartAsync(CartViewModel cartViewModel, string token)
    {
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        HttpResponseMessage response = await _client.PostAsJson($"{BasePath}/add-cart", cartViewModel);
        if (response.IsSuccessStatusCode)
            return await response.ReadContentAs<CartViewModel>();
        throw new InvalidOperationException("Something went wrong when calling API");
    }

    public async Task<CartViewModel> UpdateCartAsync(CartViewModel cartViewModel, string token)
    {
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        HttpResponseMessage response = await _client.PutAsJson($"{BasePath}/update-cart", cartViewModel);
        if (response.IsSuccessStatusCode)
            return await response.ReadContentAs<CartViewModel>();
        throw new InvalidOperationException("Something went wrong when calling API");
    }

    public async Task<bool> RemoveFromCartAsync(long cartId, string token)
    {
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        HttpResponseMessage response = await _client.DeleteAsync($"{BasePath}/remove-cart/{cartId}");
        if (response.IsSuccessStatusCode)
            return await response.ReadContentAs<bool>();
        throw new InvalidOperationException("Something went wrong when calling API");
    }

    public async Task<bool> ApplyCouponAsync(CartViewModel cartViewModel, string token)
    {
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        HttpResponseMessage response = await _client.PostAsJson($"{BasePath}/apply-coupon", cartViewModel);
        if (response.IsSuccessStatusCode)
            return await response.ReadContentAs<bool>();
        throw new InvalidOperationException("Something went wrong when calling API");
    }

    public async Task<bool> RemoveCouponAsync(string userId, string token)
    {
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        HttpResponseMessage response = await _client.DeleteAsync($"{BasePath}/remove-coupon/{userId}");
        if (response.IsSuccessStatusCode)
            return await response.ReadContentAs<bool>();
        throw new InvalidOperationException("Something went wrong when calling API");
    }

    public async Task<object> CheckoutAsync(CartHeaderViewModel cartHeaderViewModel, string token)
    {
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        HttpResponseMessage response = await _client.PostAsJson($"{BasePath}/checkout", cartHeaderViewModel);
        if (response.IsSuccessStatusCode)
            return await response.ReadContentAs<CartHeaderViewModel>();
        else if (response.StatusCode.ToString().Equals("PreconditionFailed"))
            return "Coupon Price has changed, please confirm!";

        throw new InvalidOperationException("Something went wrong when calling API");
    }

    public async Task<bool> ClearCartAsync(string userId, string token)
    {
        throw new NotImplementedException();
    }
}