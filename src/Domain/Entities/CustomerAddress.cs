using Domain.Common;
using NetTopologySuite.Geometries;

namespace Domain.Entities;

public class CustomerAddress : BaseEntity, ITenantEntity
{
    public Guid TenantId { get; set; }
    public Guid CustomerId { get; set; }
    public string? Label { get; set; }
    public string RecipientName { get; set; } = string.Empty;
    public string? RecipientPhone { get; set; }
    public string Line1 { get; set; } = string.Empty;
    public string? Line2 { get; set; }
    public string? Barangay { get; set; }
    public string City { get; set; } = string.Empty;
    public string Province { get; set; } = string.Empty;
    public string? PostalCode { get; set; }
    public string CountryCode { get; set; } = string.Empty;
    public Point? Location { get; set; } // geography
    public string? DeliveryInstructions { get; set; }
    public bool IsDefault { get; set; }

    public Tenant Tenant { get; set; } = null!;
    public Customer Customer { get; set; } = null!;
}