namespace TavosMarket.Domain.Entities;

public class CategoryFieldOption : AuditableEntityBase
{
	public Guid FieldDefinitionId { get; set; }
	public CategoryFieldDefinition FieldDefinition { get; set; } = null!;

	public string Value { get; set; } = string.Empty;   // например "BMW"
	public string Label { get; set; } = string.Empty;   // например "BMW"
	public int SortOrder { get; set; }

	public bool IsActive { get; set; } = true;
}