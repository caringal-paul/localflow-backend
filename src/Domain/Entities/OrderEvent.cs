using Domain.Common;
using Domain.Enums;
using NetTopologySuite.Geometries;


namespace Domain.Entities;

public class OrderEvent : BaseEntity, ITenantEntity
{
    public Guid TenantId { get; set; }
    public Guid OrderId { get; set; }
    public Guid? DeliveryId { get; set; } // Phase 3 (add navigation when Delivery exists)
    public string EventType { get; set; } = string.Empty;
    public string? FromStatus { get; set; }
    public string? ToStatus { get; set; }
    public string? Description { get; set; }
    public string? Data { get; set; } // jsonb
    public Point? Location { get; set; } // geography, Phase 3
    public bool IsCustomerVisible { get; set; }
    public ActorType ActorType { get; set; }
    public Guid? ActorId { get; set; }
    public DateTimeOffset OccurredAt { get; set; }

    public Tenant Tenant { get; set; } = null!;
    public Order Order { get; set; } = null!;
}