namespace TavosMarket.Domain.Entities;

public class Category : AuditableEntityBase
{
	public string Name { get; set; } = string.Empty;
	public string? Description { get; set; }

	public Guid? ParentId { get; set; }
	public Category? Parent { get; set; }
	public ICollection<Category> Children { get; set; } = new List<Category>();

	public int SortOrder { get; set; }
	public bool IsActive { get; set; } = true;
	public bool IsDirectUseForListings { get; set; }

	public ICollection<CategoryFieldDefinition> FieldDefinitions { get; set; } = new List<CategoryFieldDefinition>();
	public ICollection<Listing> Listings { get; set; } = new List<Listing>();
}