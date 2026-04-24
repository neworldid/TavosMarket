namespace TavosMarket.Domain.Entities;

public abstract class EntityBase
{
	public Guid Id { get; set; }
}

public abstract class AuditableEntityBase : EntityBase
{
	public DateTime CreatedAtUtc { get; set; }
	public Guid? CreatedById { get; set; }
	public DateTime? UpdatedAtUtc { get; set; }
	public Guid? UpdatedById { get; set; }
}