using MicroServices.OrderAPI.Model;
using MicroServices.OrderAPI.Model.Context;
using Microsoft.EntityFrameworkCore;

namespace MicroServices.OrderAPI.Repository;

public class OrderRepository : IOrderRepository
{
    private readonly DbContextOptions<SqlServerContext> _context;

    public OrderRepository(DbContextOptions<SqlServerContext> context)
    {
        _context = context;
    }

    public async Task<bool> AddOrderAsync(OrderHeader header)
    {
        if (header is null) return false;
        await using var _db = new SqlServerContext(_context);
        _db.Headers.Add(header);
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task UpdateOrderPaymentStatusAsync(long orderHeaderId, bool paid)
    {
        await using var _db = new SqlServerContext(_context);
        OrderHeader? header = await _db.Headers.FirstOrDefaultAsync(o => o.Id == orderHeaderId);

        if (header is not null)
        {
            header.PaymentStatus = paid;
            await _db.SaveChangesAsync();
        }
    }
}