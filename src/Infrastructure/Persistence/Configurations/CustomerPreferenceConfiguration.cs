using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class CustomerPreferenceConfiguration : IEntityTypeConfiguration<CustomerPreference>
{
    public void Configure(EntityTypeBuilder<CustomerPreference> builder)
    {
        builder.HasIndex(p => p.CustomerId).IsUnique().HasFilter(SqlFilters.NotDeleted);
        
        builder.HasOne(p => p.Customer)
            .WithOne(c => c.Preference)
            .HasForeignKey<CustomerPreference>(p => p.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}