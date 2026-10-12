using Application.Auth.Dtos;
using Application.Common.Errors;
using Application.Common.Interfaces;
using Application.TenantMembers;
using Application.Users;

namespace Application.Auth;

public class AuthService(
    IUserRepository users,
    ITenantMemberRepository members,
    IUnitOfWork unitOfWork,
    IPasswordService passwords,
    ICurrentUserService currentUser) : IAuthService
{
    public async Task<LoginResult> LoginAsync(LoginRequest request, CancellationToken ct)
    {
        var email = request.Email.Trim().ToLowerInvariant();

        var user = await users.GetByEmailAsync(email, ct);
        if (user?.PasswordHash is null || !passwords.Verify(user.PasswordHash, request.Password))
            throw new AppException(AuthErrors.InvalidCredentials);

        if (!user.IsActive)
            throw new AppException(AuthErrors.AccountDisabled);

        var tenantIds = await members.GetActiveTenantIdsAsync(user.Id, ct);
        if (tenantIds.Count == 0)
            throw new AppException(AuthErrors.NoTenantAccess);

        var tenantId = user.LastTenantId is { } last && tenantIds.Contains(last) ? last : tenantIds[0];

        user.LastTenantId = tenantId;
        user.LastLoginAt = DateTimeOffset.UtcNow;
        await unitOfWork.SaveChangesAsync(ct);

        return new LoginResult(user.Id, tenantId, user.SecurityStamp ?? string.Empty,
            user.Email, $"{user.FirstName} {user.LastName}");
    }

    public async Task<CurrentUserDto> GetCurrentAsync(CancellationToken ct)
    {
        var userId = currentUser.UserId ?? throw new AppException(CommonErrors.Unauthorized);

        var user = await users.GetByIdAsync(userId, ct)
            ?? throw new AppException(CommonErrors.Unauthorized);

        return new CurrentUserDto(user.Id, user.Email, $"{user.FirstName} {user.LastName}", currentUser.TenantId);
    }
}