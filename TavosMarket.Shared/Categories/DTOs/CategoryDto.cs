namespace TavosMarket.Shared.Categories.DTOs;

public class CategoryDto
{
	public Guid Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public string Slug { get; set; } = string.Empty;
	public string? Description { get; set; }
	public Guid? ParentId { get; set; }
	public string? ParentName { get; set; }
	public int SortOrder { get; set; }
	public bool IsActive { get; set; }
	public List<CategoryFieldDefinitionDto> FieldDefinitions { get; set; } = new();
	public List<CategoryDto> Children { get; set; } = new();
}
