namespace TavosMarket.Domain.Entities;

public class ListingFavorite : AuditableEntityBase
{
	public Guid UserId { get; set; }

	public Guid ListingId { get; set; }
	public Listing Listing { get; set; } = null!;
}