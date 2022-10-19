using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroServices.Email.Model;

public class EntitiesConfiguration : IEntityTypeConfiguration<EmailLog>
{
    public void Configure(EntityTypeBuilder<EmailLog> builder)
    {
        builder.HasKey(t => t.Id);
        builder.Property(p => p.Email).HasMaxLength(255).IsRequired();
        builder.Property(p => p.Log).HasMaxLength(8000).IsRequired();
        builder.Property(p => p.SentDate).HasColumnType("DateTime").HasDefaultValue(DateTime.UtcNow);
    }
}
