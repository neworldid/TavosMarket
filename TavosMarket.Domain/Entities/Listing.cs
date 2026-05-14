using Microsoft.EntityFrameworkCore;

namespace TavosMarket.Domain.Entities;

public enum ListingStatus
{
	Draft = 1,
	Published = 2,
	Rejected = 4,
	Archived = 5,
	Sold = 6,
	Expired = 7
}

[Index(nameof(Status))]
[Index(nameof(Price))]
[Index(nameof(CreatedAtUtc))]
[Index(nameof(Status), nameof(CategoryId), nameof(CreatedAtUtc))]
public class Listing : AuditableEntityBase
{
	public Guid SellerId { get; set; }

	public Guid CategoryId { get; set; }
	public Category Category { get; set; } = null!;

	public string Title { get; set; } = string.Empty;
	public string Description { get; set; } = string.Empty;

	[Precision(9, 2)]
	public decimal Price { get; set; }
	public bool IsNegotiable { get; set; }

	public ListingStatus Status { get; set; } = ListingStatus.Draft;
	public DateTime? ExpiresAt { get; set; }

	public Guid? CityId { get; set; }
	public City? City { get; set; }

	public ICollection<ListingImage> Images { get; set; } = new List<ListingImage>();
	public ICollection<ListingFieldValue> FieldValues { get; set; } = new List<ListingFieldValue>();
}