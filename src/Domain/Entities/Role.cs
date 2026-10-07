using Domain.Common;

namespace Domain.Entities;

public class Role : BaseEntity
{
    public Guid? TenantId { get; set; } // null = system role
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsSystem { get; set; }

    public Tenant? Tenant { get; set; }
    public ICollection<RolePermission> Permissions { get; set; } = [];
    public ICollection<MemberRole> MemberRoles { get; set; } = [];
}