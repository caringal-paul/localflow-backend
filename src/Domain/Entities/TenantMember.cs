
using Domain.Common;

namespace Domain.Entities;

public class TenantMember : BaseEntity, ITenantEntity
{
    public Guid TenantId { get; set; }
    public Guid UserId { get; set; }
    public bool IsActive { get; set; }
    public DateTimeOffset JoinedAt { get; set; }
    public Guid? InvitedBy { get; set; }

    public Tenant Tenant { get; set; } = null!;
    public User User { get; set; } = null!;
    public User? Inviter { get; set; }
    public ICollection<MemberRole> MemberRoles { get; set; } = [];
}