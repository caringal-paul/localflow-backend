namespace Domain.Common;

public abstract class BaseEntity : AuditEntity
{
    public Guid Id { get; set; }
}