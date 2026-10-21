using Application.Common.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class DbSeeder(ApplicationDbContext db, IPasswordService passwords)
{
    private static readonly Guid OwnerRoleId = Guid.Parse("0190f000-0000-7000-8000-000000000001");
    private static readonly Guid AdminRoleId = Guid.Parse("0190f000-0000-7000-8000-000000000002");
    private static readonly Guid StaffRoleId = Guid.Parse("0190f000-0000-7000-8000-000000000003");
    private static readonly Guid RiderRoleId = Guid.Parse("0190f000-0000-7000-8000-000000000004");

    public async Task SeedRolesAsync(CancellationToken ct)
    {
            var roles = new (Guid Id, string Code, string Name, string Description, string[] Permissions)[]
            {
                (OwnerRoleId, "owner", "Owner", "Full access",
                    ["orders.view", "orders.create", "orders.update", "orders.cancel",
                    "customers.view", "customers.manage", "products.view", "products.manage",
                    "users.view", "users.manage", "dashboard.view", "settings.manage"]),
                (AdminRoleId, "admin", "Admin", "All except tenant settings",
                    ["orders.view", "orders.create", "orders.update", "orders.cancel",
                    "customers.view", "customers.manage", "products.view", "products.manage",
                    "users.view", "users.manage", "dashboard.view"]),
                (StaffRoleId, "staff", "Staff", "Orders, customers, products (view/create)",
                    ["orders.view", "orders.create", "customers.view", "products.view", "dashboard.view"]),
                (RiderRoleId, "rider", "Rider", "Rider app access", [])
            };

            foreach (var (id, code, name, description, permissions) in roles)
        {
            if (!await db.Roles.IgnoreQueryFilters().AnyAsync(r => r.Id == id, ct))
            {
                db.Roles.Add(new Role { Id = id, TenantId = null,Code = code, Name = name, Description = description, IsSystem = true });
            }

            var existing = await db.RolePermissions.IgnoreQueryFilters()
                .Where(p => p.RoleId == id)
                .Select(p => p.Permission)
                .ToListAsync(ct);

            foreach (var permission in permissions.Except(existing))
            {
                db.RolePermissions.Add(new RolePermission { Id = Guid.CreateVersion7(), RoleId = id, Permission = permission });
            }
        }

        await db.SaveChangesAsync(ct);
    }

    public async Task SeedDevelopmentAsync(CancellationToken ct)
    {
        const string email = "owner@localflow.test";
        if (await db.Users.IgnoreQueryFilters().AnyAsync(u => u.Email == email, ct)) return;

        var tenant = new Tenant
        {
            Id = Guid.CreateVersion7(),
            Name = "Demo Company",
            Slug = "demo",
            TimeZone = "Asia/Manila",
            CurrencyCode = "PHP",
            CountryCode = "PH"
        };

        var user = new User
        {
            Id = Guid.CreateVersion7(),
            Email = email,
            FirstName = "Demo",
            LastName = "Owner",
            IsActive = true,
            SecurityStamp = Guid.NewGuid().ToString("N"),
            LastTenantId = tenant.Id
        };
        user.PasswordHash = passwords.Hash(user, "Passw0rd!");

        var member = new TenantMember
        {
            Id = Guid.CreateVersion7(),
            TenantId = tenant.Id,
            UserId = user.Id,
            Code = "MBR-0001",
            IsActive = true,
            JoinedAt = DateTimeOffset.UtcNow
        };

        foreach (var (key, prefix, next) in new[]
        {
            ("Member", "MBR-", 2L),   // MBR-0001 is assigned manually to the seeded owner
            ("Customer", "CUS-", 1L),
            ("Order", "ORD-", 1L)
        })
        {
            db.DocumentSequences.Add(new DocumentSequence
            {
                Id = Guid.CreateVersion7(),
                TenantId = tenant.Id,
                SequenceKey = key,
                Prefix = prefix,
                NextValue = next,
                Padding = 4
            });
        }
        
        db.Tenants.Add(tenant);
        db.Users.Add(user);
        db.TenantMembers.Add(member);
        db.MemberRoles.Add(new MemberRole { MemberId = member.Id, RoleId = OwnerRoleId });

        await db.SaveChangesAsync(ct);
    }
}