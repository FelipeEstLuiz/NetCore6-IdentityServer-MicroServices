using AutoMapper;
using MicroServices.CartAPI.Data.ValueObjects;
using MicroServices.CartAPI.Model;
using MicroServices.CartAPI.Model.Context;
using Microsoft.EntityFrameworkCore;

namespace MicroServices.CartAPI.Repository;

public class CartRepository : ICartRepository
{
    private readonly SqlServerContext _context;
    private readonly IMapper _mapper;

    public CartRepository(SqlServerContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<bool> ApplyCouponAsync(string userId, long couponCode)
    {
        return true;
    }

    public async Task<bool> ClearCartAsync(string userId)
    {
        return true;
    }

    public async Task<CartVO> FindCartByUserIdAsync(string userId)
    {
        Cart cart = new();

        CartHeader? cartHeader = await _context.CartHeaders.FirstOrDefaultAsync(c => c.UserId == userId);

        if (cartHeader is not null)
        {
            cart.CartHeader = cartHeader;
            cart.CartDetails = _context
                .CartDetails
                .Where(c => c.CartHeaderId == cart.CartHeader.Id)
                .Include(c => c.Product);
        }

        return _mapper.Map<CartVO>(cart);
    }

    public async Task<bool> RemoveCouponAsync(string userId)
    {
        CartHeader? cartHeader = await _context.CartHeaders.FirstOrDefaultAsync(c => c.UserId == userId);

        if (cartHeader is not null)
        {
            _context.CartDetails.RemoveRange(_context.CartDetails.Where(c => c.CartHeaderId == cartHeader.Id));
            _context.CartHeaders.Remove(cartHeader);
            await _context.SaveChangesAsync();

            return true;
        }

        return false;
    }

    public async Task<bool> RemoveFromCartAsync(long cartDetailsId)
    {
        try
        {
            CartDetail? cartDetail = await _context.CartDetails.FirstOrDefaultAsync(c => c.Id == cartDetailsId);

            if (cartDetail is null) return true;

            int total = _context.CartDetails.Where(c => c.CartHeaderId == cartDetail.CartHeaderId).Count();

            if (total == 1)
            {
                CartHeader? cartHeaderToRemove = await _context
                    .CartHeaders
                    .FirstOrDefaultAsync(c => c.Id == cartDetail.CartHeaderId);

                if (cartHeaderToRemove is not null)
                    _context.CartHeaders.Remove(cartHeaderToRemove);
            }

            await _context.SaveChangesAsync();

            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<CartVO> SaveOrUpdateCartAsync(CartVO vo)
    {
        Cart cart = _mapper.Map<Cart>(vo);

        Product? product = await _context
            .Products
            .FirstOrDefaultAsync(p => p.Id == cart.CartDetails.First().ProductId);

        if (product is null)
        {
            _context.Products.Add(cart.CartDetails.First().Product);
            await _context.SaveChangesAsync();
        }

        CartHeader? cartHeader = await _context
            .CartHeaders
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.UserId == cart.CartHeader.UserId);

        cart.CartDetails.First().Product = null;

        if (cartHeader is null)
        {
            _context.CartHeaders.Add(cart.CartHeader);
            await _context.SaveChangesAsync();
            cart.CartDetails.First().CartHeaderId = cart.CartHeader.Id;
            _context.CartDetails.Add(cart.CartDetails.First());
            await _context.SaveChangesAsync();
        }
        else
        {
            CartDetail? cartDetail = await _context
                .CartDetails
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.ProductId == cart.CartDetails.First().ProductId
                    && p.CartHeaderId == cartHeader.Id);

            if (cartDetail is null)
            {
                cart.CartDetails.First().CartHeaderId = cartHeader.Id;
            }
            else
            {
                cart.CartDetails.First().Count += cartDetail.Count;
                cart.CartDetails.First().Id = cartDetail.Id;
                cart.CartDetails.First().CartHeaderId = cartDetail.CartHeaderId;
            }

            _context.CartDetails.Add(cart.CartDetails.First());
            await _context.SaveChangesAsync();
        }

        return _mapper.Map<CartVO>(cart);
    }
}