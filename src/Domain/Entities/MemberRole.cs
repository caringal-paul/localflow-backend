using Domain.Common;

namespace Domain.Entities;

// Composite key (MemberId, RoleId) - configure in Fluent API
public class MemberRole : AuditEntity
{
    public Guid MemberId { get; set; }
    public Guid RoleId { get; set; }

    public TenantMember Member { get; set; } = null!;
    public Role Role { get; set; } = null!;
}