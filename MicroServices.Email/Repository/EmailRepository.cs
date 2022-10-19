using MicroServices.Email.Messages;
using MicroServices.Email.Model;
using MicroServices.Email.Model.Context;
using Microsoft.EntityFrameworkCore;

namespace MicroServices.Email.Repository;

public class EmailRepository : IEmailRepository
{
    private readonly DbContextOptions<SqlServerContext> _context;

    public EmailRepository(DbContextOptions<SqlServerContext> context)
    {
        _context = context;
    }

    public async Task LogEmailAsync(UpdatePaymentResultMessage message)
    {
        EmailLog email = new()
        {
            Email = message.Email,
            Log = $"Order - {message.OrderId} has been created successfully!"
        };

        await using var _db = new SqlServerContext(_context);
        _db.EmailLogs.Add(email);
        await _db.SaveChangesAsync();
    }
}