using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasIndex(u => u.Email).IsUnique().HasFilter(SqlFilters.NotDeleted);
        builder.Property(u => u.Code)
            .HasMaxLength(32)
            .HasDefaultValueSql("'USR-' || nextval('user_code_seq')");

        builder.HasIndex(u => u.Code).IsUnique().HasFilter(SqlFilters.NotDeleted);

        builder.HasOne(u => u.LastTenant)
            .WithMany()
            .HasForeignKey(u => u.LastTenantId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
