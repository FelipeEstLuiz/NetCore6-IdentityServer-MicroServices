using Microsoft.EntityFrameworkCore;

namespace MicroServices.ProductAPI.Model.Context
{
    public class SqlServerContext : DbContext
    {
        public SqlServerContext() { }
        public SqlServerContext(DbContextOptions<SqlServerContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(SqlServerContext).Assembly);
        }
    }
}
