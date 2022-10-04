using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroServices.CouponAPI.Model;

public class ProductConfiguration : IEntityTypeConfiguration<Coupon>
{
    public void Configure(EntityTypeBuilder<Coupon> builder)
    {
        builder.HasKey(t => t.Id);
        builder.Property(p => p.CouponCode).HasMaxLength(30).IsRequired();
        builder.Property(p => p.DiscountAmount).HasPrecision(10, 2).IsRequired();

        builder.HasData(new Coupon
        {
            Id = 1,
            CouponCode ="FELIPE_2022_10",
            DiscountAmount = 10
        },
        new Coupon
        {
            Id = 2,
            CouponCode = "FELIPE_2022_15",
            DiscountAmount = 15
        });
    }
}
