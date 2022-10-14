using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;

namespace MicroServices.CartAPI.Repository;

public class CouponRepository : ICouponRepository
{
    private readonly HttpClient _client;
    public const string BasePath = "api/v1/coupon";

    public CouponRepository(HttpClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async Task<CouponVO> GetCouponAsync(string couponCode, string token)
    {
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        HttpResponseMessage response = await _client.GetAsync($"api/v1/coupon/{couponCode}");

        if (response.StatusCode != HttpStatusCode.OK) return new CouponVO();

        string content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<CouponVO>(
            content,
            new JsonSerializerOptions
            { PropertyNameCaseInsensitive = true }
        );
    }
}