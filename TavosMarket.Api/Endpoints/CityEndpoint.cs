using Microsoft.AspNetCore.Mvc;
using TavosMarket.Application.Services;
using TavosMarket.Shared;

namespace TavosMarket.Api.Endpoints;

public class CityEndpoint : IEndpoint
{
	public static void MapEndpoint(IEndpointRouteBuilder builder)
	{
		builder.MapGet(ApiRoutes.Cities.Base, GetCities);
	}

	private static async Task<IResult> GetCities([FromQuery] string? search, CityService cityService, CancellationToken ct)
	{
		return TypedResults.Ok(await cityService.GetCitiesAsync(search, ct));
	}
}
