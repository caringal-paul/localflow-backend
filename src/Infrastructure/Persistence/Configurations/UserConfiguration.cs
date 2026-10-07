using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasIndex(u => u.Email).IsUnique().HasFilter(SqlFilters.NotDeleted);

        builder.HasOne(u => u.LastTenant)
            .WithMany()
            .HasForeignKey(u => u.LastTenantId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
