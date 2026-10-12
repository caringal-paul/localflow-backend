namespace Application.TenantMembers;

public interface ITenantMemberRepository
{
    /// <summary>Active tenants the user can sign in to, oldest membership first.</summary>
    Task<IReadOnlyList<Guid>> GetActiveTenantIdsAsync(Guid userId, CancellationToken ct);
}