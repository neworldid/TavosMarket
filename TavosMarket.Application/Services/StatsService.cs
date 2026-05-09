using Microsoft.EntityFrameworkCore;
using TavosMarket.Application.Auth;
using TavosMarket.Application.Interfaces;
using TavosMarket.Domain.Entities;
using TavosMarket.Shared.Stats.DTOs;

namespace TavosMarket.Application.Services;

public class StatsService(
    ITavosMarketDbContext dbContext, 
    ICurrentUserService currentUserService,
    IUserService userService)
{
    public async Task<PublicStatisticsDto> GetPublicStatsAsync(CancellationToken ct = default)
    {
        return new PublicStatisticsDto
        {
            TotalListings = await dbContext.Listings.CountAsync(l => l.Status == ListingStatus.Published, ct),
            TotalCategories = await dbContext.Categories.CountAsync(ct)
        };
    }

    public async Task<UserStatisticsDto> GetUserStatsAsync(CancellationToken ct = default)
    {
        var userId = currentUserService.UserId;
        if (userId == null || userId == Guid.Empty) return new UserStatisticsDto();

        return new UserStatisticsDto
        {
            MyListingsCount = await dbContext.Listings.CountAsync(l => l.SellerId == userId, ct),
            MyActiveListingsCount = await dbContext.Listings.CountAsync(l => l.SellerId == userId && l.Status == ListingStatus.Published, ct)
        };
    }

    public async Task<AdminStatisticsDto> GetAdminStatsAsync(CancellationToken ct = default)
    {
        return new AdminStatisticsDto
        {
            TotalListings = await dbContext.Listings.CountAsync(ct),
            PendingModerationListings = await dbContext.Listings.CountAsync(l => l.Status == ListingStatus.Moderation, ct),
            TotalCategories = await dbContext.Categories.CountAsync(ct),
            TotalUsers = await userService.GetTotalUsersCountAsync(ct)
        };
    }
}
