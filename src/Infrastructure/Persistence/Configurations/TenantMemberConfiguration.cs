using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class TenantMemberConfiguration : IEntityTypeConfiguration<TenantMember>
{
    public void Configure(EntityTypeBuilder<TenantMember> builder)
    {
        builder.HasIndex(m => new { m.TenantId, m.UserId }).IsUnique().HasFilter(SqlFilters.NotDeleted);

        builder.HasOne(m => m.Tenant)
            .WithMany(t => t.Members)
            .HasForeignKey(m => m.TenantId);

        builder.HasOne(m => m.User)
            .WithMany(u => u.Memberships)
            .HasForeignKey(m => m.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(m => m.Inviter)
            .WithMany()
            .HasForeignKey(m => m.InvitedBy)
            .OnDelete(DeleteBehavior.SetNull);
    }
}