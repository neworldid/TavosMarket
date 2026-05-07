using TavosMarket.Shared.Enums;

namespace TavosMarket.Shared.Categories.DTOs;

public class CategoryFieldDefinitionDto
{
	public Guid Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public string? Description { get; set; }
	public FieldDataTypeDto DataTypeDto { get; set; }
	public bool IsRequired { get; set; }
	public bool IsFilterable { get; set; }
	public int SortOrder { get; set; }
	public string? Unit { get; set; }
	public string? Placeholder { get; set; }
	public decimal? MinValue { get; set; }
	public decimal? MaxValue { get; set; }
	public List<CategoryFieldOptionDto> Options { get; set; } = new();
}
