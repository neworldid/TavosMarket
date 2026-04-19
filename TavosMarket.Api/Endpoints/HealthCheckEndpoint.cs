using JetBrains.Annotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;

namespace TavosMarket.Api.Endpoints;

/// <inheritdoc />
public class HealthCheckEndpoint : IEndpoint
{
    /// <inheritdoc />
    public static void MapEndpoint(IEndpointRouteBuilder builder)
    {
        var group = builder.MapGroup("api");
        group.MapGet("health", HealthCheck).WithName("Health");
    }

    /// <summary>
    /// Health check
    /// </summary>
    /// <returns>OK</returns>
    [PublicAPI]
    [Authorize]
    public static Ok<string> HealthCheck()
    {
        return TypedResults.Ok("OK");
    }
}
