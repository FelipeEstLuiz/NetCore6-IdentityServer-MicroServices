using MicroServices.CartAPI.Data.ValueObjects;
using MicroServices.CartAPI.Messages;
using MicroServices.CartAPI.RabbitMQSender;
using MicroServices.CartAPI.Repository;
using Microsoft.AspNetCore.Mvc;

namespace MicroServices.CartAPI.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class CartController : ControllerBase
{
    private readonly ICartRepository _cartRepository;
    private readonly ICouponRepository _couponRepository;
    private readonly IRabbitMQMessageSender _rabbitMQMessageSender;

    public CartController(
        ICartRepository cartRepository,
        IRabbitMQMessageSender rabbitMQMessageSender,
        ICouponRepository couponRepository)
    {
        _cartRepository = cartRepository
            ?? throw new ArgumentNullException(nameof(cartRepository));

        _rabbitMQMessageSender = rabbitMQMessageSender
            ?? throw new ArgumentNullException(nameof(rabbitMQMessageSender));

        _couponRepository = couponRepository
            ?? throw new ArgumentNullException(nameof(couponRepository));
    }

    [HttpGet("find-cart/{id}")]
    public async Task<ActionResult<CartVO>> FindById(string id)
    {
        CartVO cart = await _cartRepository.FindCartByUserIdAsync(id);
        if (cart is null) return NotFound();
        return Ok(cart);
    }

    [HttpPost("add-cart")]
    public async Task<ActionResult<CartVO>> AddCart(CartVO vo)
    {
        CartVO cart = await _cartRepository.SaveOrUpdateCartAsync(vo);
        if (cart == null) return NotFound();
        return Ok(cart);
    }

    [HttpPut("update-cart")]
    public async Task<ActionResult<CartVO>> UpdateCart(CartVO vo)
    {
        CartVO cart = await _cartRepository.SaveOrUpdateCartAsync(vo);
        if (cart is null) return NotFound();
        return Ok(cart);
    }

    [HttpDelete("remove-cart/{id}")]
    public async Task<ActionResult<CartVO>> RemoveCart(int id)
    {
        bool status = await _cartRepository.RemoveFromCartAsync(id);
        if (!status) return BadRequest();
        return Ok(status);
    }

    [HttpPost("apply-coupon")]
    public async Task<ActionResult<CartVO>> ApplyCoupon(CartVO vo)
    {
        bool status = await _cartRepository.ApplyCouponAsync(vo.CartHeader.UserId, vo.CartHeader.CouponCode);
        if (!status) return NotFound();
        return Ok(status);
    }

    [HttpDelete("remove-coupon/{userId}")]
    public async Task<ActionResult<CartVO>> ApplyCoupon(string userId)
    {
        bool status = await _cartRepository.RemoveCouponAsync(userId);
        if (!status) return NotFound();
        return Ok(status);
    }

    [HttpPost("checkout")]
    public async Task<ActionResult<CheckoutHeaderVO>> Checkout(CheckoutHeaderVO vo)
    {
        if (vo?.UserId is null) return BadRequest();
        CartVO cart = await _cartRepository.FindCartByUserIdAsync(vo.UserId);
        if (cart is null) return NotFound();

        if (!string.IsNullOrEmpty(vo.CouponCode))
        {
            string token = Request.Headers["Authorization"];
            CouponVO coupon = await _couponRepository.GetCouponAsync(vo.CouponCode, token);

            if (vo.DiscountAmount != coupon.DiscountAmount)
                return StatusCode(412);
        }

        vo.CartDetails = cart.CartDetails;
        vo.DateTime = DateTime.Now;

        _rabbitMQMessageSender.SendMessage(vo, "checkoutqueue");

        return Ok(vo);
    }
}
