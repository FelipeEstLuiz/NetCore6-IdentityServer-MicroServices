using MicroServices.CartAPI.Messages;
using MicroServices.MessageBus;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace MicroServices.CartAPI.RabbitMQSender;

public class RabbitMQMessageSender : IRabbitMQMessageSender
{
    private readonly string _hostName;
    private readonly string _password;
    private readonly string _userName;

    public RabbitMQMessageSender()
    {
        _hostName = "localhost";
        _password = "guest";
        _userName = "guest";
    }

    public void SendMessage(BaseMessage baseMessage, string queueName)
    {
        ConnectionFactory factory = new()
        {
            HostName = _hostName,
            UserName = _userName,
            Password = _password
        };

        IConnection connection = factory.CreateConnection();

        using IModel channel = connection.CreateModel();
        channel.QueueDeclare(queue: queueName, false, false, false, arguments: null);
        byte[] body = GetMessageAsByteArray(baseMessage);
        channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);
    }

    private static byte[] GetMessageAsByteArray(BaseMessage baseMessage)
    {
        JsonSerializerOptions options = new()
        {
            WriteIndented = true
        };
        string json = JsonSerializer.Serialize((CheckoutHeaderVO)baseMessage, options: options);
        return Encoding.UTF8.GetBytes(json);
    }
}
