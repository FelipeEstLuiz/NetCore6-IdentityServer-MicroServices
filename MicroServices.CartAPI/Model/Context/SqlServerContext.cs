using Microsoft.EntityFrameworkCore;

namespace MicroServices.CartAPI.Model.Context;

public class SqlServerContext : DbContext
{
    public SqlServerContext(DbContextOptions<SqlServerContext> options) : base(options) { }

    public DbSet<Product> Products { get; set; }
    public DbSet<CartDetail> CartDetails { get; set; }
    public DbSet<CartHeader> CartHeaders { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SqlServerContext).Assembly);
    }
}
