namespace TavosMarket.Domain.Entities;

public class ListingImage : EntityBase
{
	public Guid ListingId { get; set; }
	public Listing Listing { get; set; } = null!;

	public string Url { get; set; } = string.Empty;
	public string? ThumbnailUrl { get; set; }

	public int SortOrder { get; set; }
	public bool IsMain { get; set; }
}