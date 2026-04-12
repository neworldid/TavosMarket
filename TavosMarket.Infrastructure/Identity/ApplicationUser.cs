using Microsoft.AspNetCore.Identity;
using TavosMarket.Application.Auth;

namespace TavosMarket.Infrastructure.Identity;

public class ApplicationUser : IdentityUser<Guid>, ITokenUser
{
	public string? FirstName { get; set; }
	public string? LastName { get; set; }

	Guid ITokenUser.Id => Id;
}