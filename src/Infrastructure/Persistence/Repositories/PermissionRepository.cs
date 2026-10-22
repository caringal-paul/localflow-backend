using Application.Auth;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class PermissionRepository(ApplicationDbContext db) : IPermissionRepository
{
    // Tenant filter limits members to the current tenant. Soft-delete filters apply automatically.
    public async Task<IReadOnlyList<string>> GetForUserAsync(Guid userId, CancellationToken ct) =>
        await db.TenantMembers
            .AsNoTracking()
            .Where(m => m.UserId == userId && m.IsActive)
            .SelectMany(m => m.MemberRoles)
            .Join(db.RolePermissions, mr => mr.RoleId, p => p.RoleId, (mr, p) => p.Permission)
            .Distinct()
            .ToListAsync(ct);
}