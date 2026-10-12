using Application.Auth.Dtos;
using Application.Common.Errors;
using Application.Common.Interfaces;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Application.Auth;

public class AuthService(IApplicationDbContext db, IPasswordService passwords, ICurrentUserService currentUser)
    : IAuthService
{
    public async Task<LoginResult> LoginAsync(LoginRequest request, CancellationToken ct)
    {
        var email = request.Email.Trim().ToLowerInvariant();

        var user = await db.Users.FirstOrDefaultAsync(u => u.Email == email, ct);
        if (user?.PasswordHash is null || !passwords.Verify(user.PasswordHash, request.Password))
            throw new AppException(AuthErrors.InvalidCredentials);

        if (!user.IsActive)
            throw new AppException(AuthErrors.AccountDisabled);

        // No tenant yet, so bypass the tenant filter. This also drops soft-delete, so it is repeated by hand.
        var tenantIds = await db.TenantMembers
            .IgnoreQueryFilters()
            .Where(m => m.UserId == user.Id && m.IsActive && !m.IsDeleted
                        && !m.Tenant.IsDeleted && m.Tenant.Status == TenantStatus.Active)
            .OrderBy(m => m.JoinedAt)
            .Select(m => m.TenantId)
            .ToListAsync(ct);

        if (tenantIds.Count == 0)
            throw new AppException(AuthErrors.NoTenantAccess);

        var tenantId = user.LastTenantId is { } last && tenantIds.Contains(last) ? last : tenantIds[0];

        user.LastTenantId = tenantId;
        user.LastLoginAt = DateTimeOffset.UtcNow;
        await db.SaveChangesAsync(ct);

        return new LoginResult(user.Id, tenantId, user.SecurityStamp ?? string.Empty,
            user.Email, $"{user.FirstName} {user.LastName}");
    }

    public async Task<CurrentUserDto> GetCurrentAsync(CancellationToken ct)
    {
        var userId = currentUser.UserId ?? throw new AppException(CommonErrors.Unauthorized);

        var user = await db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == userId, ct)
            ?? throw new AppException(CommonErrors.Unauthorized);

        return new CurrentUserDto(user.Id, user.Email, $"{user.FirstName} {user.LastName}", currentUser.TenantId);
    }
}