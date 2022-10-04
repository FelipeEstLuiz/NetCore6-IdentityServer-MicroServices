using MicroServices.CouponAPI.Data.ValueObjects;
using MicroServices.CouponAPI.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MicroServices.CouponAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CouponController : ControllerBase
{
    public readonly ICouponRepository _repository;

    public CouponController(ICouponRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    [HttpGet("{couponCode}")]
    [Authorize]
    public async Task<ActionResult<CouponVO>> GetCouponByCouponCode(string couponCode)
    {
        CouponVO? coupon = await _repository.GetCouponByCouponCodeAsync(couponCode);
        if(coupon is null) return NotFound();
        return Ok(coupon);
    }
}
