using JetBrains.Annotations;
using ServiceScan.SourceGenerator;
using TavosMarket.Api.Endpoints;

namespace TavosMarket.Api;

/// <summary>
/// Automatically register endpoints
/// </summary>
/// <see href="https://renatogolia.com/2025/08/07/auto-register-aspnet-core-minimal-api-endpoints">Source</see>
public static partial class HttpEndpointServiceCollectionExtensions
{
	/// <summary>
	/// Maps endpoints in the app
	/// </summary>
	/// <param name="builder">Endpoint Route Builder</param>
	/// <returns>Endpoint Route Builder</returns>
	[ScanForTypes(AssignableTo = typeof(IEndpoint), Handler = nameof(MapEndpoint))]
	[PublicAPI]
	public static partial IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder builder);

	/// <summary>
	/// Invokes the static <c>MapEndpoint</c> method on the specified endpoint type
	/// to register its routes on the provided builder.
	/// </summary>
	/// <param name="builder">Endpoint route builder.</param>
	/// <typeparam name="T">Endpoint type implementing <see cref="IEndpoint"/>.</typeparam>
	private static void MapEndpoint<T>(IEndpointRouteBuilder builder) where T : IEndpoint
	{
		T.MapEndpoint(builder);
	}
}
