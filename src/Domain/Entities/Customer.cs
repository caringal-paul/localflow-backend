using Domain.Common;
using Domain.Enums;

namespace Domain.Entities;

public class Customer : BaseEntity, ITenantEntity
{
    public Guid TenantId { get; set; }
    public string Code { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string? CompanyName { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? AlternatePhone { get; set; }
    public string? Notes { get; set; }
    public NotificationChannels AllowedChannels { get; set; }
    public bool IsActive { get; set; }

    public Tenant Tenant { get; set; } = null!;
    public ICollection<CustomerAddress> Addresses { get; set; } = [];
    public ICollection<Order> Orders { get; set; } = [];
}