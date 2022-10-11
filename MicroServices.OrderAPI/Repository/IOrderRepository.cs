using MicroServices.OrderAPI.Model;

namespace MicroServices.OrderAPI.Repository;

public interface IOrderRepository
{
    Task<bool> AddOrderAsync(OrderHeader header);
    Task UpdateOrderPaymentStatusAsync(long orderHeaderId, bool paid);
}