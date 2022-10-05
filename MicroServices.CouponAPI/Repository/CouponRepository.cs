using AutoMapper;
using MicroServices.CouponAPI.Data.ValueObjects;
using MicroServices.CouponAPI.Model;
using MicroServices.CouponAPI.Model.Context;
using Microsoft.EntityFrameworkCore;

namespace MicroServices.CouponAPI.Repository;

public class CouponRepository : ICouponRepository
{
    private readonly SqlServerContext _context;
    private readonly IMapper _mapper;

    public CouponRepository(SqlServerContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<CouponVO> GetCouponByCouponCodeAsync(string couponCode)
    {
        Coupon? coupon = await _context.Coupons.FirstOrDefaultAsync(c => c.CouponCode == couponCode);
        return _mapper.Map<CouponVO>(coupon);
    }
}