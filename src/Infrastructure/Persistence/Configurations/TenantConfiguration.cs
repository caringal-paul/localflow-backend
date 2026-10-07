using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class TenantConfiguration : IEntityTypeConfiguration<Tenant>
{
    public void Configure(EntityTypeBuilder<Tenant> builder)
    {
        builder.HasIndex(t => t.Slug).IsUnique().HasFilter(SqlFilters.NotDeleted);

        builder.Property(t => t.CountryCode).HasMaxLength(2);
        builder.Property(t => t.CurrencyCode).HasMaxLength(3);
        builder.Property(t => t.Settings).HasColumnType("jsonb");
    }
}