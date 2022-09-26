using AutoMapper;
using MicroServices.CartAPI.Data.ValueObjects;
using MicroServices.CartAPI.Model.Context;

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
        throw new NotImplementedException();
    }

    public async Task<bool> ClearCartAsync(string userId)
    {
        throw new NotImplementedException();
    }

    public async Task<CartVO> FindCartByUserIdAsync(string userId)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> RemoveCouponAsync(string userId)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> RemoveFromCartAsync(long cartDetailsId)
    {
        throw new NotImplementedException();
    }

    public async Task<CartVO> SaveOrUpdateCartAsync(CartVO cart)
    {
        throw new NotImplementedException();
    }
}