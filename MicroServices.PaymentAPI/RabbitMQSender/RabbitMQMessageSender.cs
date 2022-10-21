using MicroServices.MessageBus;
using MicroServices.PaymentAPI.Messages;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace MicroServices.PaymentAPI.RabbitMQSender;

public class RabbitMQMessageSender : IRabbitMQMessageSender
{
    private readonly string _hostName;
    private readonly string _password;
    private readonly string _userName;
    private IConnection _connection;
    private const string ExchangeName = "DirectPaymantUpdateExchange";
    private const string PaymentEmailUpdateQueueName = "PaymentEmailUpdateQueueName";
    private const string PaymentOrderUpdateQueueName = "PaymentOrderUpdateQueueName";

    public RabbitMQMessageSender()
    {
        _hostName = "localhost";
        _password = "guest";
        _userName = "guest";
    }

    public void SendMessage(BaseMessage baseMessage)
    {
        if (ConnectionExists())
        {
            using IModel channel = _connection.CreateModel();

            channel.ExchangeDeclare(ExchangeName, ExchangeType.Direct, durable: false);

            channel.QueueDeclare(PaymentEmailUpdateQueueName, false, false, false, null);
            channel.QueueDeclare(PaymentOrderUpdateQueueName, false, false, false, null);

            channel.QueueBind(PaymentEmailUpdateQueueName, ExchangeName, "PaymentEmail");
            channel.QueueBind(PaymentOrderUpdateQueueName, ExchangeName, "PaymentOrder");

            byte[] body = GetMessageAsByteArray(baseMessage);

            channel.BasicPublish(
                exchange: ExchangeName,
                routingKey: "PaymentEmail",
                basicProperties: null,
                body: body
            );
            
            channel.BasicPublish(
                exchange: ExchangeName,
                routingKey: "PaymentOrder",
                basicProperties: null,
                body: body
            );
        }
    }

    private static byte[] GetMessageAsByteArray(BaseMessage baseMessage)
    {
        JsonSerializerOptions options = new()
        {
            WriteIndented = true
        };
        string json = JsonSerializer.Serialize((UpdatePaymentResultMessage)baseMessage, options: options);
        return Encoding.UTF8.GetBytes(json);
    }

    private bool ConnectionExists()
    {
        if (_connection is not null) return true;
        CreateConnection();
        return _connection is not null;
    }

    private void CreateConnection()
    {
        try
        {
            ConnectionFactory factory = new()
            {
                HostName = _hostName,
                UserName = _userName,
                Password = _password
            };

            _connection = factory.CreateConnection();
        }
        catch (Exception)
        {
            //Log error
            throw;
        }
    }
}
