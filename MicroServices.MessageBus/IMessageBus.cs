namespace MicroServices.MessageBus;

public interface IMessageBus
{
    Task PublicMessage(BaseMessage message, string queueName);
}