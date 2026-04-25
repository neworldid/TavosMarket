namespace TavosMarket.Shared.Categories.DTOs;

public class CategoryFieldOptionDto
{
	public Guid Id { get; set; }
	public string Value { get; set; } = string.Empty;
	public int SortOrder { get; set; }
}
