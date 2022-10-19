using System.Text;
using System.Text.Json;
using MicroServices.PaymentAPI.Messages;
using MicroServices.PaymentAPI.RabbitMQSender;
using MicroServices.PaymentProcessor;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace MicroServices.PaymentAPI.MessageConsumer;

public class RabbitMQPaymentConsumer : BackgroundService
{
    private readonly IModel _channel;
    private IConnection _connection;
    private readonly IProcessPayment _processPayment;
    private readonly IRabbitMQMessageSender _rabbitMQMessageSender;

    public RabbitMQPaymentConsumer(
        IProcessPayment processPayment,
        IRabbitMQMessageSender rabbitMQMessageSender
    )
    {
        _rabbitMQMessageSender = rabbitMQMessageSender;
        ConnectionFactory factory = new()
        {
            HostName = "localhost",
            UserName = "guest",
            Password = "guest"
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _channel.QueueDeclare(queue: "orderpaymentprocessqueue", false, false, false, arguments: null);
        _processPayment = processPayment;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();
        EventingBasicConsumer consumer = new(_channel);
        consumer.Received += (chanel, evt) =>
        {
            string content = Encoding.UTF8.GetString(evt.Body.ToArray());
            PaymentMessage? vo = JsonSerializer.Deserialize<PaymentMessage>(content);
            ProcessPayment(vo).GetAwaiter().GetResult();
            _channel.BasicAck(evt.DeliveryTag, false);
        };
        _channel.BasicConsume("orderpaymentprocessqueue", false, consumer);
        return Task.CompletedTask;
    }

    private async Task ProcessPayment(PaymentMessage? vo)
    {
        if (vo != null)
        {
            bool result = await _processPayment.PaymentProcessor();

            UpdatePaymentResultMessage paymentResult = new()
            {
                Status = result,
                OrderId = vo.OrderId,
                Email = vo.Email
            };

            try
            {
                _rabbitMQMessageSender.SendMessage(paymentResult);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}