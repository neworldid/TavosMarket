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
        authGroup.MapGet(ApiRoutes.Listings.MyListings, GetMyListings);
        authGroup.MapPost(ApiRoutes.Listings.Create, Create);
        authGroup.MapPut(ApiRoutes.Listings.Update, Update);
        authGroup.MapDelete(ApiRoutes.Listings.Delete, Delete);
    }

    private static async Task<IResult> GetAll(
        [FromQuery] Guid? categoryId,
        [FromQuery] decimal? minPrice,
        [FromQuery] decimal? maxPrice,
        [FromQuery] ListingStatusDto? status,
        [FromQuery(Name = "f")] string[]? f,
        ListingService listingService, 
        CancellationToken ct)
    {
        var fieldFilters = new Dictionary<Guid, string>();
        if (f != null)
        {
            foreach (var filterStr in f)
            {
                var parts = filterStr.Split(':', 2);
                if (parts.Length == 2 && Guid.TryParse(parts[0], out var fieldId))
                {
                    fieldFilters[fieldId] = parts[1];
                }
            }
        }

        var filter = new ListingFilterDto
        {
            CategoryId = categoryId,
            MinPrice = minPrice,
            MaxPrice = maxPrice,
            Status = status,
            FieldFilters = fieldFilters
        };
        return TypedResults.Ok(await listingService.GetListingsAsync(filter, ct));
    }

    private static async Task<IResult> GetMyListings(ListingService listingService, CancellationToken ct)
    {
        return TypedResults.Ok(await listingService.GetCurrentUserListingsAsync(ct));
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
