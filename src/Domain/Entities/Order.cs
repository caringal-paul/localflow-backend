

using Domain.Common;
using Domain.Enums;
using NetTopologySuite.Geometries;

namespace Domain.Entities;

public class Order : BaseEntity, ITenantEntity
{
    public Guid TenantId { get; set; }
    public string OrderNumber { get; set; } = string.Empty;
    public Guid CustomerId { get; set; }
    public OrderStatus Status { get; set; }
    public string? Priority { get; set; }
    public string? SourceChannel { get; set; }
    public string? ExternalReference { get; set; }
    public string TrackingToken { get; set; } = string.Empty;

    // Delivery snapshot
    public Guid? CustomerAddressId { get; set; }
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
    public DateOnly? RequestedDeliveryDate { get; set; }
    public TimeOnly? RequestedWindowStart { get; set; }
    public TimeOnly? RequestedWindowEnd { get; set; }

    // Totals
    public string CurrencyCode { get; set; } = string.Empty;
    public decimal Subtotal { get; set; }
    public decimal DiscountTotal { get; set; }
    public decimal TaxTotal { get; set; }
    public decimal ShippingFee { get; set; }
    public decimal GrandTotal { get; set; }

    // Payment
    public PaymentMethod PaymentMethod { get; set; }
    public string PaymentStatus { get; set; } = string.Empty;
    public decimal? CodAmount { get; set; }

    public decimal? TotalWeightKg { get; set; }
    public decimal? TotalVolumeCm3 { get; set; }

    public DateTimeOffset? PlacedAt { get; set; }
    public DateTimeOffset? ConfirmedAt { get; set; }
    public DateTimeOffset? CancelledAt { get; set; }
    public string? CancelReason { get; set; }
    public string? Notes { get; set; }
    public string? InternalNotes { get; set; }

    public Tenant Tenant { get; set; } = null!;
    public Customer Customer { get; set; } = null!;
    public CustomerAddress? CustomerAddress { get; set; }
    public ICollection<OrderItem> Items { get; set; } = [];
    public ICollection<OrderEvent> Events { get; set; } = [];
}