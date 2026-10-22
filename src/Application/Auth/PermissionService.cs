using Application.Common.Interfaces;

namespace Application.Auth;

public class PermissionService(ICurrentUserService currentUser, IPermissionRepository permissions) : IPermissionService
{
    private HashSet<string>? _cache;

    public async Task<bool> HasAsync(string permission, CancellationToken ct)
    {
        if (currentUser.UserId is not { } userId) return false;

        _cache ??= (await permissions.GetForUserAsync(userId, ct)).ToHashSet();
        return _cache.Contains(permission);
    }
}