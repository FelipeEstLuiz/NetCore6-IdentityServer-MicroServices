using Microsoft.EntityFrameworkCore;

namespace MicroServices.OrderAPI.Model.Context;

public class SqlServerContext : DbContext
{
    public SqlServerContext(DbContextOptions<SqlServerContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SqlServerContext).Assembly);
    }

    public DbSet<OrderDetail> Details { get; set; }
    public DbSet<OrderHeader> Headers { get; set; }
}
