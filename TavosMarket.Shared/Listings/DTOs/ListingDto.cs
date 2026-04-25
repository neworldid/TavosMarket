using TavosMarket.Shared.Categories.DTOs;

namespace TavosMarket.Shared.Listings.DTOs;

public class ListingDto
{
    public Guid Id { get; set; }
    public Guid SellerId { get; set; }
    public Guid CategoryId { get; set; }
    public CategoryDto? Category { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public bool IsNegotiable { get; set; }
    public string? City { get; set; }
    public string? Region { get; set; }
    public List<ListingFieldValueDto> FieldValues { get; set; } = new();
}

public class ListingFieldValueDto
{
    public Guid Id { get; set; }
    public Guid FieldDefinitionId { get; set; }
    public string? StringValue { get; set; }
    public decimal? DecimalValue { get; set; }
    public int? IntValue { get; set; }
    public bool? BoolValue { get; set; }
    public DateTime? DateValue { get; set; }
    public Guid? OptionId { get; set; }
    
    // For UI binding
    public CategoryFieldDefinitionDto? FieldDefinition { get; set; }
}
