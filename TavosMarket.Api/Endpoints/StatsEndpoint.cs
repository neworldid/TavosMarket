using TavosMarket.Application.Services;
using TavosMarket.Shared;

namespace TavosMarket.Api.Endpoints;

public class StatsEndpoint : IEndpoint
{
    public static void MapEndpoint(IEndpointRouteBuilder builder)
    {
        var group = builder.MapGroup("");

        group.MapGet(ApiRoutes.Stats.Public, GetPublicStats);
        
        var authGroup = builder.MapGroup("").RequireAuthorization();
        authGroup.MapGet(ApiRoutes.Stats.User, GetUserStats);
        
        var adminGroup = builder.MapGroup("").RequireAuthorization(policy => policy.RequireRole("Admin"));
        adminGroup.MapGet(ApiRoutes.Stats.Admin, GetAdminStats);
    }

    private static async Task<IResult> GetPublicStats(StatsService statsService, CancellationToken ct)
    {
        return TypedResults.Ok(await statsService.GetPublicStatsAsync(ct));
    }

    private static async Task<IResult> GetUserStats(StatsService statsService, CancellationToken ct)
    {
        return TypedResults.Ok(await statsService.GetUserStatsAsync(ct));
    }

    private static async Task<IResult> GetAdminStats(StatsService statsService, CancellationToken ct)
    {
        return TypedResults.Ok(await statsService.GetAdminStatsAsync(ct));
    }
}
