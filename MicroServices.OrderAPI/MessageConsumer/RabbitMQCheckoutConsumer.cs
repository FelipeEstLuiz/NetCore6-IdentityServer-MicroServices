using System.Text;
using System.Text.Json;
using MicroServices.OrderAPI.Messages;
using MicroServices.OrderAPI.Model;
using MicroServices.OrderAPI.Repository;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace MicroServices.OrderAPI.MessageConsumer;

public class RabbitMQCheckoutConsumer : BackgroundService
{
    private readonly OrderRepository _repository;
    private readonly IModel _channel;

    public RabbitMQCheckoutConsumer(OrderRepository repository)
    {
        _repository = repository;

        ConnectionFactory factory = new()
        {
            HostName = "localhost",
            UserName = "guest",
            Password = "guest"
        };

        IConnection connection = factory.CreateConnection();

        _channel = connection.CreateModel();
        _channel.QueueDeclare(queue: "checkoutqueue", false, false, false, arguments: null);
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();
        EventingBasicConsumer consumer = new(_channel);
        consumer.Received += (chanel, evt) =>
        {
            string content = Encoding.UTF8.GetString(evt.Body.ToArray());
            CheckoutHeaderVO? vo = JsonSerializer.Deserialize<CheckoutHeaderVO>(content);
            ProcessOrder(vo).GetAwaiter().GetResult();
            _channel.BasicAck(evt.DeliveryTag, false);
        };
        _channel.BasicConsume("checkoutqueue", false, consumer);
        return Task.CompletedTask;
    }

    private async Task ProcessOrder(CheckoutHeaderVO? vo)
    {
        if (vo != null)
        {
            OrderHeader order = new()
            {
                UserId = vo.UserId,
                CardNumber = vo.CardNumber,
                CartTotalItens = vo.CartTotalItens,
                CouponCode = vo.CouponCode,
                CVV = vo.CVV,
                DateTime = vo.DateTime,
                DiscountAmount = vo.DiscountAmount,
                Email = vo.Email,
                FirstName = vo.FirstName,
                LastName = vo.LastName,
                Id = vo.Id,
                OrderDetails = new List<OrderDetail>(),
                Phone = vo.Phone,
                ExpiryMonthYear = vo.ExpiryMonthYear,
                OrderTime = DateTime.Now,
                PaymentStatus = false,
                PurchaseAmount = vo.PurchaseAmount
            };

            if (vo.CartDetails?.Any() == true)
            {
                foreach (CartDetailVO details in vo.CartDetails)
                {
                    OrderDetail orderDetail = new()
                    {
                        ProductId = details.ProductId,
                        ProductName = details.Product?.Name,
                        Price = details.Product?.Price ?? 0,
                        Count = details.Count
                    };
                    order.CartTotalItens += details.Count;
                    order.OrderDetails.Add(orderDetail);
                }
            }

            await _repository.AddOrderAsync(order);
        }
    }
}