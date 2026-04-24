using Microsoft.EntityFrameworkCore;

namespace TavosMarket.Domain.Entities;

public class ListingFieldValue : AuditableEntityBase
{
	public Guid ListingId { get; set; }
	[DeleteBehavior(DeleteBehavior.NoAction)]
	public Listing Listing { get; set; } = null!;

	public Guid FieldDefinitionId { get; set; }
	public CategoryFieldDefinition FieldDefinition { get; set; } = null!;

	public string? StringValue { get; set; }
	[Precision(18, 3)]
	public decimal? DecimalValue { get; set; }
	public int? IntValue { get; set; }
	public bool? BoolValue { get; set; }
	public DateTime? DateValue { get; set; }

	public Guid? OptionId { get; set; }
	public CategoryFieldOption? Option { get; set; }
}