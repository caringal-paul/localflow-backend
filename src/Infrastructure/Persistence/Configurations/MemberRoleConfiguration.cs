using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class MemberRoleConfiguration : IEntityTypeConfiguration<MemberRole>
{
    public void Configure(EntityTypeBuilder<MemberRole> builder)
    {
        builder.HasKey(mr => new { mr.MemberId, mr.RoleId });

        builder.HasOne(mr => mr.Member)
            .WithMany(m => m.MemberRoles)
            .HasForeignKey(mr => mr.MemberId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(mr => mr.Role)
            .WithMany(r => r.MemberRoles)
            .HasForeignKey(mr => mr.RoleId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}