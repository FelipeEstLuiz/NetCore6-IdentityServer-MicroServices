using System.Text;
using System.Text.Json;
using MicroServices.OrderAPI.Messages;
using MicroServices.OrderAPI.Repository;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace MicroServices.OrderAPI.MessageConsumer;

public class RabbitMQPaymentConsumer : BackgroundService
{
    private readonly OrderRepository _repository;
    private readonly IModel _channel;
    private IConnection _connection;
    private const string ExchangeName = "DirectPaymantUpdateExchange";
    private const string PaymentOrderUpdateQueueName = "PaymentOrderUpdateQueueName";

    public RabbitMQPaymentConsumer(OrderRepository repository)
    {
        _repository = repository;

        ConnectionFactory factory = new()
        {
            HostName = "localhost",
            UserName = "guest",
            Password = "guest"
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        _channel.ExchangeDeclare(ExchangeName, ExchangeType.Direct);
        _channel.QueueDeclare(PaymentOrderUpdateQueueName, false, false, false, null);
        _channel.QueueBind(PaymentOrderUpdateQueueName, ExchangeName, "PaymentOrder");
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();
        EventingBasicConsumer consumer = new(_channel);
        consumer.Received += (chanel, evt) =>
        {
            string content = Encoding.UTF8.GetString(evt.Body.ToArray());
            UpdatePaymentResultVO? vo = JsonSerializer.Deserialize<UpdatePaymentResultVO>(content);
            UpdatePaymentStatus(vo).GetAwaiter().GetResult();
            _channel.BasicAck(evt.DeliveryTag, false);
        };
        _channel.BasicConsume(PaymentOrderUpdateQueueName, false, consumer);
        return Task.CompletedTask;
    }

    private async Task UpdatePaymentStatus(UpdatePaymentResultVO? vo)
    {
        try
        {
            if (vo is not null)
                await _repository.UpdateOrderPaymentStatusAsync(vo.OrderId, vo.Status);
        }
        catch (Exception)
        {
            throw;
        }
    }
}