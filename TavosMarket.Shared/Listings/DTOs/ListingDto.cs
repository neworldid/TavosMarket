using TavosMarket.Shared.Categories.DTOs;

namespace TavosMarket.Shared.Listings.DTOs;

public enum ListingStatusDto
{
    Draft = 1,
    Published = 2,
    Moderation = 3,
    Rejected = 4,
    Archived = 5,
    Sold = 6
}

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
    public ListingStatusDto Status { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    
    public List<ListingImageDto> Images { get; set; } = new();
    public List<ListingFieldValueDto> FieldValues { get; set; } = new();
}

public class ListingImageDto
{
    public Guid Id { get; set; }
    public string Url { get; set; } = string.Empty;
    public string? ThumbnailUrl { get; set; }
    public int SortOrder { get; set; }
    public bool IsMain { get; set; }
    
    // For uploading new images
    public byte[]? Data { get; set; }
    public string? FileName { get; set; }
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
    public List<Guid> OptionIds { get; set; } = new();
    
    // For UI binding
    public CategoryFieldDefinitionDto? FieldDefinition { get; set; }
}
