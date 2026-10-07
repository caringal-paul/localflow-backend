using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasIndex(p => new { p.TenantId, p.Sku }).IsUnique().HasFilter(SqlFilters.NotDeleted);

        builder.Property(p => p.TaxRate).HasPrecision(5, 2);
        builder.Property(p => p.WeightKg).HasPrecision(10, 3);
        builder.Property(p => p.LengthCm).HasPrecision(10, 2);
        builder.Property(p => p.WidthCm).HasPrecision(10, 2);
        builder.Property(p => p.HeightCm).HasPrecision(10, 2);

        builder.HasOne(p => p.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}