using MicroServices.MessageBus;

namespace MicroServices.PaymentAPI.RabbitMQSender;

public interface IRabbitMQMessageSender
{
    void SendMessage(BaseMessage baseMessage,string queueName);
}
