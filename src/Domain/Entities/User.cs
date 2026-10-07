using Domain.Common;

namespace Domain.Entities;

public class User : BaseEntity
{
    public string Email { get; set; } = string.Empty;
    public string? PasswordHash { get; set; }
    public string? SecurityStamp { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? AvatarUrl { get; set; }
    public bool IsActive { get; set; }
    public DateTimeOffset? LastLoginAt { get; set; }
    public Guid? LastTenantId { get; set; }

    public Tenant? LastTenant { get; set; }
    public ICollection<TenantMember> Memberships { get; set; } = [];
}