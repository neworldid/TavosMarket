namespace TavosMarket.Domain.Entities;

public class ListingMessage : AuditableEntityBase
{
	public Guid ListingId { get; set; }
	public Guid SenderId { get; set; }

	public string Text { get; set; } = string.Empty;

	public bool IsRead { get; set; }
}