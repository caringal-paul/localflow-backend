
using Domain.Common;

namespace Domain.Entities;

public class OrderItem : BaseEntity, ITenantEntity
{
    public Guid TenantId { get; set; }
    public Guid OrderId { get; set; }
    public Guid ProductId { get; set; }
    public string Sku { get; set; } = string.Empty; // snapshot
    public string ProductName { get; set; } = string.Empty; // snapshot
    public string? Unit { get; set; }
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal LineTotal { get; set; }
    public decimal? WeightKg { get; set; }
    public string? Notes { get; set; }

    public Tenant Tenant { get; set; } = null!;
    public Order Order { get; set; } = null!;
    public Product Product { get; set; } = null!;
}