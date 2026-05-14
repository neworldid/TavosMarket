using Microsoft.EntityFrameworkCore;
using TavosMarket.Domain.Enums;

namespace TavosMarket.Domain.Entities;

[Index(nameof(CategoryId), nameof(SortOrder))]
public class CategoryFieldDefinition : AuditableEntityBase
{
	public Guid CategoryId { get; set; }
	public Category Category { get; set; } = null!;

	public string Name { get; set; } = string.Empty;
	public string? Description { get; set; }

	public FieldDataType DataType { get; set; }

	public bool IsRequired { get; set; }
	public bool IsFilterable { get; set; }

	public int SortOrder { get; set; }

	public string? Unit { get; set; }
	public string? Placeholder { get; set; }

	[Precision(18, 3)]
	public decimal? MinValue { get; set; }
	[Precision(18, 3)]
	public decimal? MaxValue { get; set; }

	public ICollection<CategoryFieldOption> Options { get; set; } = new List<CategoryFieldOption>();
}