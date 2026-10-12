using Application.TenantMembers;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class TenantMemberRepository(ApplicationDbContext db) : ITenantMemberRepository
{
    public async Task<IReadOnlyList<Guid>> GetActiveTenantIdsAsync(Guid userId, CancellationToken ct)
        // No tenant in context yet, so bypass the tenant filter. This also drops soft-delete, so it is repeated by hand.
        => await db.TenantMembers
            .IgnoreQueryFilters()
            .Where(m => m.UserId == userId && m.IsActive && !m.IsDeleted
                        && !m.Tenant.IsDeleted && m.Tenant.Status == TenantStatus.Active)
            .OrderBy(m => m.JoinedAt)
            .Select(m => m.TenantId)
            .ToListAsync(ct);
}