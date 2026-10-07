
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasIndex(o => new { o.TenantId, o.OrderNumber }).IsUnique().HasFilter(SqlFilters.NotDeleted);
        builder.HasIndex(o => o.TrackingToken).IsUnique();
        builder.HasIndex(o => new { o.TenantId, o.Status });
        builder.HasIndex(o => new { o.TenantId, o.CustomerId });

        builder.Property(o => o.CountryCode).HasMaxLength(2);
        builder.Property(o => o.CurrencyCode).HasMaxLength(3);
        builder.Property(o => o.TotalWeightKg).HasPrecision(10, 3);

        builder.HasOne(o => o.Customer)
            .WithMany(c => c.Orders)
            .HasForeignKey(o => o.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(o => o.CustomerAddress)
            .WithMany()
            .HasForeignKey(o => o.CustomerAddressId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}