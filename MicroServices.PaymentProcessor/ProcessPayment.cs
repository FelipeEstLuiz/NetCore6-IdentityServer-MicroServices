namespace MicroServices.PaymentProcessor;

public class ProcessPayment : IProcessPayment
{
    public Task<bool> PaymentProcessor()
    {
        return Task.FromResult(true);
    }
}
