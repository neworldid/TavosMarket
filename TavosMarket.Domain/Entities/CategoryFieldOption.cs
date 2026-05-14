using Microsoft.EntityFrameworkCore;

namespace TavosMarket.Domain.Entities;

[Index(nameof(FieldDefinitionId), nameof(SortOrder))]
public class CategoryFieldOption : AuditableEntityBase
{
	public Guid FieldDefinitionId { get; set; }
	public CategoryFieldDefinition FieldDefinition { get; set; } = null!;

	public string Value { get; set; } = string.Empty;
	public string Label { get; set; } = string.Empty;
	public int SortOrder { get; set; }

	public bool IsActive { get; set; } = true;
}