namespace TavosMarket.Shared.Stats.DTOs;

public class PublicStatisticsDto
{
    public int TotalListings { get; set; }
    public int TotalCategories { get; set; }
}

public class UserStatisticsDto
{
    public int MyListingsCount { get; set; }
    public int MyActiveListingsCount { get; set; }
}

public class AdminStatisticsDto
{
    public int TotalUsers { get; set; }
    public int TotalListings { get; set; }
    public int PendingModerationListings { get; set; }
    public int TotalCategories { get; set; }
}
