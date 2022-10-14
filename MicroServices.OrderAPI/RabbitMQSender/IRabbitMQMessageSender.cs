using MicroServices.MessageBus;

namespace MicroServices.OrderAPI.RabbitMQSender;

public interface IRabbitMQMessageSender
{
    void SendMessage(BaseMessage baseMessage,string queueName);
}
