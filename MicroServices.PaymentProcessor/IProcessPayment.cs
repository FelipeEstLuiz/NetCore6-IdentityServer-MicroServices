namespace MicroServices.PaymentProcessor;

public interface IProcessPayment
{
    Task<bool> PaymentProcessor();
}
