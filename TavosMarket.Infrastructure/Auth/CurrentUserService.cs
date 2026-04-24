using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using TavosMarket.Application.Auth;

namespace TavosMarket.Infrastructure.Auth;

public class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{

	public Guid? UserId
	{
		get
		{
			var value = httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

			return Guid.TryParse(value, out var userId)
				? userId
				: null;
		}
	}
	public string? Email { get; }
	public bool IsAuthenticated => httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated == true;
}