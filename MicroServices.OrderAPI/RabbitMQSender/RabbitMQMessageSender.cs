using MicroServices.MessageBus;
using MicroServices.OrderAPI.Messages;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace MicroServices.OrderAPI.RabbitMQSender;

public class RabbitMQMessageSender : IRabbitMQMessageSender
{
    private readonly string _hostName;
    private readonly string _password;
    private readonly string _userName;
    private IConnection _connection;

    public RabbitMQMessageSender()
    {
        _hostName = "localhost";
        _password = "guest";
        _userName = "guest";
    }

    public void SendMessage(BaseMessage baseMessage, string queueName)
    {
        if (ConnectionExists())
        {
            using IModel channel = _connection.CreateModel();
            channel.QueueDeclare(queue: queueName, false, false, false, arguments: null);
            byte[] body = GetMessageAsByteArray(baseMessage);
            channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);
        }
    }

    private static byte[] GetMessageAsByteArray(BaseMessage baseMessage)
    {
        JsonSerializerOptions options = new()
        {
            WriteIndented = true
        };
        string json = JsonSerializer.Serialize((PaymentVO)baseMessage, options: options);
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
