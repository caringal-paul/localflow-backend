namespace Application.Auth;

public interface IPermissionService
{
    Task<bool> HasAsync(string permission, CancellationToken ct);
}