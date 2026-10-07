
using Domain.Common;

namespace Domain.Entities;

public class Product : BaseEntity, ITenantEntity
{
    public Guid TenantId { get; set; }
    public Guid? CategoryId { get; set; }
    public string Sku { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Barcode { get; set; }
    public decimal Price { get; set; }
    public decimal? CostPrice { get; set; }
    public decimal? TaxRate { get; set; }
    public string? Unit { get; set; }
    public decimal? WeightKg { get; set; }
    public decimal? LengthCm { get; set; }
    public decimal? WidthCm { get; set; }
    public decimal? HeightCm { get; set; }
    public bool IsFragile { get; set; }
    public string? ImageUrl { get; set; }
    public bool TrackInventory { get; set; }
    public bool IsActive { get; set; }

    public Tenant Tenant { get; set; } = null!;
    public ProductCategory? Category { get; set; }
}