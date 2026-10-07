using Domain.Common;

namespace Domain.Entities;

public class ProductCategory : BaseEntity, ITenantEntity
{
    public Guid TenantId { get; set; }
    public Guid? ParentId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int SortOrder { get; set; }

    public Tenant Tenant { get; set; } = null!;
    public ProductCategory? Parent { get; set; }
    public ICollection<ProductCategory> Children { get; set; } = [];
    public ICollection<Product> Products { get; set; } = [];
}