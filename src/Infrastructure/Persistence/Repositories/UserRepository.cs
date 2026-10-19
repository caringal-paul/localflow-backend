using Application.Users;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class UserRepository(ApplicationDbContext db) : IUserRepository
{
    public Task<User?> GetByEmailAsync(string email, CancellationToken ct)
        => db.Users.FirstOrDefaultAsync(u => u.Email == email, ct);

    public Task<User?> GetByIdAsync(Guid id, CancellationToken ct)
        => db.Users.FirstOrDefaultAsync(u => u.Id == id, ct);

    public Task<bool> IsSessionValidAsync(Guid userId, string? securityStamp, CancellationToken ct)
        => db.Users.AsNoTracking()
            .AnyAsync(u => u.Id == userId && u.IsActive && u.SecurityStamp == securityStamp, ct);

    public async Task<IReadOnlyList<TenantMember>> ListMembersAsync(CancellationToken ct) =>
        await db.TenantMembers
            .AsNoTracking()
            .Include(m => m.User)
            .Include(m => m.MemberRoles)
            .OrderBy(m => m.User.FirstName)
            .ToListAsync(ct);

    public Task<TenantMember?> GetMemberAsync(Guid userId, CancellationToken ct) =>
        db.TenantMembers
            .Include(m => m.User)
            .Include(m => m.MemberRoles)
            .FirstOrDefaultAsync(m => m.UserId == userId, ct);

    public async Task AddMemberAsync(TenantMember member, CancellationToken ct) =>
        await db.TenantMembers.AddAsync(member, ct);

    public void RemoveMember(TenantMember member) => db.TenantMembers.Remove(member);
}