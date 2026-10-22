namespace Application.Auth;

public interface IPermissionRepository
{
    Task<IReadOnlyList<string>> GetForUserAsync(Guid userId, CancellationToken ct);
}