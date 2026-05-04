using System.Security.Claims;
using TavosMarket.Application.Auth;
using TavosMarket.Shared;
using TavosMarket.Shared.Auth.DTOs;

namespace TavosMarket.Api.Endpoints;

public class AuthEndpoint : IEndpoint
{
	public static void MapEndpoint(IEndpointRouteBuilder builder)
	{
		builder.MapPost(ApiRoutes.Auth.Register, Register);
		builder.MapPost(ApiRoutes.Auth.Login, Login);
		builder.MapGet(ApiRoutes.Auth.CurrentUser, CurrentUser);
		builder.MapPost(ApiRoutes.Auth.GoogleLogin, GoogleLogin);
	}
	
	private static async Task<IResult> Register(
		RegisterRequest request,
		IAuthService authService,
		CancellationToken cancellationToken)
	{
		try
		{
			var result = await authService.RegisterAsync(request, cancellationToken);
			return TypedResults.Created(ApiRoutes.Auth.CurrentUser, result);
		}
		catch (InvalidOperationException ex)
		{
			return TypedResults.BadRequest(new { error = ex.Message });
		}
	}

	private static async Task<IResult> Login(
		LoginRequest request,
		IAuthService authService,
		CancellationToken cancellationToken)
	{
		try
		{
			var result = await authService.LoginAsync(request, cancellationToken);
			return TypedResults.Ok(result);
		}
		catch (InvalidOperationException ex)
		{
			return TypedResults.BadRequest(new { error = ex.Message });
		}
	}

	private static async Task<IResult> CurrentUser(
		ClaimsPrincipal user,
		IAuthService authService,
		CancellationToken cancellationToken)
	{
		var userIdValue = user.FindFirstValue(ClaimTypes.NameIdentifier);
		if (!Guid.TryParse(userIdValue, out var userId))
			return Results.Unauthorized();

		var result = await authService.CurrentUserAsync(userId, cancellationToken);
		return TypedResults.Ok(result);
	}

	private static async Task<IResult> GoogleLogin(
		GoogleLoginRequest request,
		IAuthService authService,
		CancellationToken cancellationToken)
	{
		try
		{
			var result = await authService.GoogleLoginAsync(request, cancellationToken);
			return TypedResults.Ok(result);
		}
		catch (InvalidOperationException ex)
		{
			return TypedResults.BadRequest(new { error = ex.Message });
		}
	}
}