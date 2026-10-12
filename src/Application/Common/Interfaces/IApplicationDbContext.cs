using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Interfaces;

public interface IApplicationDbContext
{
    // Phase 1
    DbSet<Tenant> Tenants { get; }
    DbSet<DocumentSequence> DocumentSequences { get; }
    DbSet<User> Users { get; }
    DbSet<TenantMember> TenantMembers { get; }
    DbSet<Role> Roles { get; }
    DbSet<RolePermission> RolePermissions { get; }
    DbSet<MemberRole> MemberRoles { get; }
    DbSet<UserInvitation> UserInvitations { get; }
    DbSet<Customer> Customers { get; }
    DbSet<CustomerAddress> CustomerAddresses { get; }
    DbSet<ProductCategory> ProductCategories { get; }
    DbSet<Product> Products { get; }
    DbSet<Order> Orders { get; }
    DbSet<OrderItem> OrderItems { get; }
    DbSet<OrderEvent> OrderEvents { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}