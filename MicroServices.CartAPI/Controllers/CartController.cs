using MicroServices.CartAPI.Data.ValueObjects;
using MicroServices.CartAPI.Repository;
using Microsoft.AspNetCore.Mvc;

namespace MicroServices.CartAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartRepository _cartRepository;

        public CartController(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository ?? throw new ArgumentNullException(nameof(cartRepository));
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
            if (cart is null) return NotFound();
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
            if (status) return BadRequest();
            return Ok(status);
        }
    }
}
