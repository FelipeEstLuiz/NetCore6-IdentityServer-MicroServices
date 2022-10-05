using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroServices.CartAPI.Model;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(t => t.Id);
        builder.Property(p => p.Name).HasMaxLength(150).IsRequired();
        builder.Property(p => p.Price).HasPrecision(20, 2).IsRequired();
        builder.Property(p => p.Description).HasMaxLength(500);
        builder.Property(p => p.CategoryName).HasMaxLength(50);
        builder.Property(p => p.ImageURL).HasMaxLength(300);
    }
}
