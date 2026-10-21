using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        // Tenant roles
        builder.HasIndex(r => new { r.TenantId, r.Name }).IsUnique().HasFilter(SqlFilters.NotDeleted);

        builder.Property(r => r.Code).HasMaxLength(64);

        builder.HasIndex(r => new { r.TenantId, r.Code })
            .IsUnique()
            .HasFilter("is_deleted = false AND tenant_id IS NOT NULL");

        builder.HasIndex(r => r.Code)
            .IsUnique()
            .HasFilter("is_deleted = false AND tenant_id IS NULL");

        // System roles (TenantId null): Postgres treats NULLs as distinct, so enforce separately
        builder.HasIndex(r => r.Name)
            .IsUnique()
            .HasFilter("tenant_id IS NULL AND is_deleted = false")
            .HasDatabaseName("ix_roles_name_system");

        builder.HasOne(r => r.Tenant)
            .WithMany()
            .HasForeignKey(r => r.TenantId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}