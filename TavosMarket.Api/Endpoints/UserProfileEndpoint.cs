using Microsoft.AspNetCore.Mvc;
using TavosMarket.Application.Interfaces;
using TavosMarket.Shared;
using TavosMarket.Shared.Auth.DTOs;

namespace TavosMarket.Api.Endpoints;

public class UserProfileEndpoint : IEndpoint
{
    public static void MapEndpoint(IEndpointRouteBuilder builder)
    {
        var group = builder.MapGroup("").RequireAuthorization();

        group.MapGet(ApiRoutes.UserProfile.Base, GetProfile);
        group.MapPut(ApiRoutes.UserProfile.Update, UpdateProfile);
    }

    private static async Task<IResult> GetProfile(IUserService userService, CancellationToken ct)
    {
        var profile = await userService.GetProfileAsync(ct);
        return profile is not null ? TypedResults.Ok(profile) : TypedResults.NotFound();
    }

    private static async Task<IResult> UpdateProfile(
        [FromBody] UserProfileDto request, 
        IUserService userService, 
        CancellationToken ct)
    {
        try
        {
            var result = await userService.UpdateProfileAsync(request, ct);
            return TypedResults.Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            return TypedResults.Forbid();
        }
        catch (InvalidOperationException ex)
        {
            return TypedResults.BadRequest(ex.Message);
        }
    }
}
