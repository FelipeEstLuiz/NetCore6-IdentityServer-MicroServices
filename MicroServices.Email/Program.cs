using MicroServices.Email.MessageConsumer;
using MicroServices.Email.Model.Context;
using MicroServices.Email.Repository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
string connectionString = builder.Configuration["ConnectionStrings:DefaultConnection"];

builder.Services.AddDbContext<SqlServerContext>(
    options => options.UseSqlServer(
        connectionString,
        b => b.MigrationsAssembly(typeof(SqlServerContext).Assembly.FullName)
    )
);

var contex = new DbContextOptionsBuilder<SqlServerContext>();
contex.UseSqlServer(connectionString);

builder.Services.AddSingleton(new EmailRepository(contex.Options));
builder.Services.AddScoped<IEmailRepository, EmailRepository>();
builder.Services.AddHostedService<RabbitMQPaymentConsumer>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
