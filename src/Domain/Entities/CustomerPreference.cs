using Domain.Common;

namespace Domain.Entities;

public class CustomerPreference : BaseEntity, ITenantEntity
{
    public Guid TenantId { get; set; }
    public Guid CustomerId { get; set; }
    public bool AllowEmail { get; set; } = true;
    public bool AllowSms { get; set; } = true;
    public bool AllowPush { get; set; } = true;

    public Customer Customer { get; set; } = null!;
}