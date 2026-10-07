using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class UserInvitationConfiguration : IEntityTypeConfiguration<UserInvitation>
{
    public void Configure(EntityTypeBuilder<UserInvitation> builder)
    {
        builder.HasIndex(i => i.TokenHash).IsUnique();

        builder.HasOne(i => i.Role)
            .WithMany()
            .HasForeignKey(i => i.RoleId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(i => i.Inviter)
            .WithMany()
            .HasForeignKey(i => i.InvitedBy)
            .OnDelete(DeleteBehavior.Restrict);
    }
}