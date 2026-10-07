using System.Reflection;
using Application.Common.Interfaces;
using Domain.Common;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;

namespace Infrastructure.Persistence;

public class ApplicationDbContext(
    DbContextOptions<ApplicationDbContext> options,
    ICurrentUserService currentUser) : DbContext(options)
{
    // Instance property so EF parameterizes the filter per context instance
    private Guid? CurrentTenantId => currentUser.TenantId;

    // Phase 1
    public DbSet<Tenant> Tenants => Set<Tenant>();
    public DbSet<DocumentSequence> DocumentSequences => Set<DocumentSequence>();
    public DbSet<User> Users => Set<User>();
    public DbSet<TenantMember> TenantMembers => Set<TenantMember>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<RolePermission> RolePermissions => Set<RolePermission>();
    public DbSet<MemberRole> MemberRoles => Set<MemberRole>();
    public DbSet<UserInvitation> UserInvitations => Set<UserInvitation>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<CustomerAddress> CustomerAddresses => Set<CustomerAddress>();
    public DbSet<ProductCategory> ProductCategories => Set<ProductCategory>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<OrderEvent> OrderEvents => Set<OrderEvent>();

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        // Every enum in Domain: [Flags] -> int, otherwise -> string
        var enums = typeof(User).Assembly.GetTypes().Where(t => t.IsEnum);
        foreach (var type in enums)
        {
            if (type.IsDefined(typeof(FlagsAttribute), false))
                configurationBuilder.Properties(type).HaveConversion(typeof(int));
            else
                configurationBuilder.Properties(type).HaveConversion<string>().HaveMaxLength(32);
        }

        // Spatial: every Point is a PostGIS geography (SRID 4326)
        configurationBuilder.Properties<Point>().HaveColumnType("geography (point, 4326)");

        // Money default; override per property (quantity, weight, dimensions, tax rate)
        configurationBuilder.Properties<decimal>().HavePrecision(18, 2);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.HasPostgresExtension("postgis");
        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        // Soft-delete + tenant filter + row version for every AuditEntity
        var auditTypes = builder.Model.GetEntityTypes()
            .Select(t => t.ClrType)
            .Where(t => typeof(AuditEntity).IsAssignableFrom(t))
            .ToList();

        foreach (var type in auditTypes)
            ConfigureAuditEntityMethod.MakeGenericMethod(type).Invoke(this, [builder]);

        // Role: system roles (TenantId null) are visible to everyone
        builder.Entity<Role>().HasQueryFilter(r =>
            !r.IsDeleted && (r.TenantId == null || r.TenantId == CurrentTenantId));

        // Tenant ownership FKs never cascade
        var tenantFks = builder.Model.GetEntityTypes()
            .SelectMany(e => e.GetForeignKeys())
            .Where(fk => fk.Properties.Count == 1 && fk.Properties[0].Name == nameof(ITenantEntity.TenantId));

        foreach (var fk in tenantFks)
            fk.DeleteBehavior = DeleteBehavior.Restrict;
    }

    private static readonly MethodInfo ConfigureAuditEntityMethod =
        typeof(ApplicationDbContext).GetMethod(nameof(ConfigureAuditEntity), BindingFlags.NonPublic | BindingFlags.Instance)!;

    private void ConfigureAuditEntity<T>(ModelBuilder builder) where T : AuditEntity
    {
        var entity = builder.Entity<T>();
        entity.Property(e => e.RowVersion).IsRowVersion(); // Npgsql -> xmin

        if (typeof(ITenantEntity).IsAssignableFrom(typeof(T)))
            entity.HasQueryFilter(e => !e.IsDeleted && EF.Property<Guid>(e, nameof(ITenantEntity.TenantId)) == CurrentTenantId);
        else
            entity.HasQueryFilter(e => !e.IsDeleted);
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        ApplyAuditing();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        ApplyAuditing();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    private void ApplyAuditing()
    {
        var now = DateTimeOffset.UtcNow;
        var userId = currentUser.UserId;

        foreach (var entry in ChangeTracker.Entries().ToList())
        {
            // Stamp TenantId on insert unless explicitly set (e.g. creating a tenant, seeding)
            if (entry is { State: EntityState.Added, Entity: ITenantEntity tenantEntity }
                && tenantEntity.TenantId == Guid.Empty)
            {
                tenantEntity.TenantId = currentUser.TenantId
                    ?? throw new InvalidOperationException("TenantId is required to create this record.");
            }

            if (entry.Entity is not AuditEntity audit) continue;

            switch (entry.State)
            {
                case EntityState.Added:
                    audit.CreatedAt = now;
                    audit.CreatedBy = userId;
                    break;
                case EntityState.Modified:
                    audit.UpdatedAt = now;
                    audit.UpdatedBy = userId;
                    break;
                case EntityState.Deleted: // soft delete
                    entry.State = EntityState.Modified;
                    audit.IsDeleted = true;
                    audit.UpdatedAt = now;
                    audit.UpdatedBy = userId;
                    break;
            }
        }
    }
}