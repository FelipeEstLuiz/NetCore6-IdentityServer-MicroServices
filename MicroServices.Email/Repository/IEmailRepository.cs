using MicroServices.Email.Messages;

namespace MicroServices.Email.Repository;

public interface IEmailRepository
{
    Task LogEmailAsync(UpdatePaymentResultMessage message);
}