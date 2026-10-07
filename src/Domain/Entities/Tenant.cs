using Domain.Common;
using Domain.Enums;

namespace Domain.Entities;

public class Tenant : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string TimeZone { get; set; } = string.Empty;
    public string CurrencyCode { get; set; } = string.Empty;
    public string CountryCode { get; set; } = string.Empty;
    public string? LogoUrl { get; set; }
    public TenantStatus Status { get; set; }
    public string? Settings { get; set; }

    public ICollection<TenantMember> Members { get; set; } = [];
}