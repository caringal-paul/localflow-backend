using Domain.Common;

namespace Domain.Entities;

public class UserInvitation : BaseEntity, ITenantEntity
{
    public Guid TenantId { get; set; }
    public string Email { get; set; } = string.Empty;
    public Guid RoleId { get; set; }
    public Guid InvitedBy { get; set; }
    public string TokenHash { get; set; } = string.Empty;
    public DateTimeOffset ExpiresAt { get; set; }
    public DateTimeOffset? AcceptedAt { get; set; }

    public Tenant Tenant { get; set; } = null!;
    public Role Role { get; set; } = null!;
    public User Inviter { get; set; } = null!;
}