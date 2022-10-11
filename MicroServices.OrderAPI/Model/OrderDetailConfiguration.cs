using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroServices.OrderAPI.Model;

public class OrderDetailConfiguration : IEntityTypeConfiguration<OrderDetail>
{
    public void Configure(EntityTypeBuilder<OrderDetail> builder)
    {
        builder.HasKey(t => t.Id);
        builder.Property(p => p.ProductId).IsRequired();
        builder.Property(p => p.Count).IsRequired();
        builder.Property(p => p.ProductName).HasMaxLength(150).IsRequired();
        builder.Property(p => p.Price).HasPrecision(10, 2).IsRequired();
    }
}
