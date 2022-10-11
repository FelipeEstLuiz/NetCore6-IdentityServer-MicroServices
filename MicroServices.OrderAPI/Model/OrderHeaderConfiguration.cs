using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroServices.OrderAPI.Model;

public class OrderHeaderConfiguration : IEntityTypeConfiguration<OrderHeader>
{
    public void Configure(EntityTypeBuilder<OrderHeader> builder)
    {
        builder.HasKey(t => t.Id);
        builder.Property(p => p.CouponCode).HasMaxLength(30).IsRequired();
        builder.Property(p => p.UserId).HasMaxLength(100).IsRequired();
        builder.Property(p => p.DiscountAmount).HasPrecision(10, 2).IsRequired();
        builder.Property(p => p.PurchaseAmount).HasPrecision(10, 2).IsRequired();
        builder.Property(p => p.FirstName).HasMaxLength(255);
        builder.Property(p => p.LastName).HasMaxLength(255);
        builder.Property(p => p.DateTime).IsRequired();
        builder.Property(p => p.OrderTime).IsRequired();
        builder.Property(p => p.Phone).HasMaxLength(20);
        builder.Property(p => p.Email).HasMaxLength(30);
        builder.Property(p => p.CardNumber).HasMaxLength(20);
        builder.Property(p => p.CVV).HasMaxLength(10);
        builder.Property(p => p.ExpiryMonthYear).HasMaxLength(10);
        builder.Property(p => p.CartTotalItens).IsRequired();
        builder.Property(p => p.PaymentStatus).IsRequired();
    }
}
