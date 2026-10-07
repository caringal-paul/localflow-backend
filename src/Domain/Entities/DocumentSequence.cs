using Domain.Common;

namespace Domain.Entities;

public class DocumentSequence : BaseEntity, ITenantEntity
{
    public Guid TenantId { get; set; }
    public string SequenceKey { get; set; } = string.Empty;
    public string? Prefix { get; set; }
    public long NextValue { get; set; }
    public int Padding { get; set; }

    public Tenant Tenant { get; set; } = null!;
}