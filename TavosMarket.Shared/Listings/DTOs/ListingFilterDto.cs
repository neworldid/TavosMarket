namespace TavosMarket.Shared.Listings.DTOs;

public class ListingFilterDto
{
    public Guid? CategoryId { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public Dictionary<Guid, string>? FieldFilters { get; set; }
}
