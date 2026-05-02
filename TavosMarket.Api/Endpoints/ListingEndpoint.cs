using Microsoft.AspNetCore.Mvc;
using TavosMarket.Application.Services;
using TavosMarket.Shared;
using TavosMarket.Shared.Listings.DTOs;

namespace TavosMarket.Api.Endpoints;

public class ListingEndpoint : IEndpoint
{
    public static void MapEndpoint(IEndpointRouteBuilder builder)
    {
        var group = builder.MapGroup("");

        group.MapGet(ApiRoutes.Listings.Base, GetAll);
        group.MapGet(ApiRoutes.Listings.GetById, GetById);
        
        var authGroup = builder.MapGroup("").RequireAuthorization();
        authGroup.MapPost(ApiRoutes.Listings.Create, Create);
        authGroup.MapPut(ApiRoutes.Listings.Update, Update);
        authGroup.MapDelete(ApiRoutes.Listings.Delete, Delete);
    }

    private static async Task<IResult> GetAll(
        [FromQuery] Guid? categoryId, 
        ListingService listingService, 
        CancellationToken ct)
    {
        return TypedResults.Ok(await listingService.GetListingsAsync(categoryId, ct));
    }

    private static async Task<IResult> GetById(Guid id, ListingService listingService, CancellationToken ct)
    {
        var listing = await listingService.GetByIdAsync(id, ct);
        return listing is not null ? TypedResults.Ok(listing) : TypedResults.NotFound();
    }

    private static async Task<IResult> Create([FromBody] ListingDto request, ListingService listingService, CancellationToken ct)
    {
        var result = await listingService.CreateAsync(request, ct);
        return TypedResults.Created($"{ApiRoutes.Listings.Base}/{result.Id}", result);
    }

    private static async Task<IResult> Update(Guid id, [FromBody] ListingDto request, ListingService listingService, CancellationToken ct)
    {
        await listingService.UpdateAsync(id, request, ct);
        return TypedResults.NoContent();
    }

    private static async Task<IResult> Delete(Guid id, ListingService listingService, CancellationToken ct)
    {
        await listingService.DeleteAsync(id, ct);
        return TypedResults.NoContent();
    }
}
