namespace TavosMarket.Api.Endpoints;

public interface IEndpoint
{
	static abstract void MapEndpoint(IEndpointRouteBuilder builder);
}