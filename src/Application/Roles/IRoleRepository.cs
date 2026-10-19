namespace Application.Roles;

public interface IRoleRepository
{
    Task<bool> ExistsAsync(Guid id, CancellationToken ct);
}