using MicroServices.MessageBus;

namespace MicroServices.CartAPI.RabbitMQSender;

public interface IRabbitMQMessageSender
{
    void SendMessage(BaseMessage baseMessage,string queueName);
}
