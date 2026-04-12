using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TavosMarket.Application.Auth;
using TavosMarket.Infrastructure.Identity;
using TavosMarket.Shared.Auth.DTOs;

namespace TavosMarket.Infrastructure.Auth;

public sealed class AuthService(
	UserManager<ApplicationUser> userManager,
	SignInManager<ApplicationUser> signInManager,
	IJwtTokenService jwtTokenService) : IAuthService
{
	private const string DefaultUserRole = "User";
	public async Task<AuthResponse> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken)
	{
		var user = new ApplicationUser
		{
			Id = Guid.NewGuid(),
			Email = request.Email,
			UserName = request.UserName,
			FirstName = request.FirstName,
			LastName = request.LastName,
			EmailConfirmed = true
		};
		try
		{
			var existingUser = await userManager.FindByEmailAsync(request.Email);
			if (existingUser is not null)
				throw new InvalidOperationException("User already exists.");

			var result = await userManager.CreateAsync(user, request.Password);
			if (!result.Succeeded)
				throw new InvalidOperationException(string.Join("; ", result.Errors.Select(x => x.Description)));

			await userManager.AddToRoleAsync(user, DefaultUserRole);

			return await CreateAuthResponseAsync(user, cancellationToken);
		}
		catch (InvalidOperationException)
		{
			throw;
		}
		catch (Exception ex)
		{
			throw new InvalidOperationException("Registration failed.", ex);
		}
		
	}

	public async Task<AuthResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken)
	{
		var user = await userManager.FindByEmailAsync(request.Email);
		if (user is null)
			throw new InvalidOperationException("Invalid email or password.");

		var passwordOk = await signInManager.CheckPasswordSignInAsync(user, request.Password, lockoutOnFailure: false);
		if (!passwordOk.Succeeded)
			throw new InvalidOperationException("Invalid email or password.");

		return await CreateAuthResponseAsync(user, cancellationToken);
	}

	public async Task<UserDto> CurrentUserAsync(Guid userId, CancellationToken cancellationToken)
	{
		var user = await userManager.Users
			.AsNoTracking()
			.FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);

		if (user is null)
			throw new InvalidOperationException("User not found.");

		return MapUser(user);
	}

	public async Task<AuthResponse> GoogleLoginAsync(GoogleLoginRequest request, CancellationToken cancellationToken)
	{
		throw new NotImplementedException();
	}

	private async Task<AuthResponse> CreateAuthResponseAsync(ApplicationUser user, CancellationToken cancellationToken)
	{
		var roles = await userManager.GetRolesAsync(user);

		var extraClaims = roles.Select(role => new Claim(ClaimTypes.Role, role)).ToList();
		var accessToken = jwtTokenService.CreateAccessToken(user, extraClaims);

		return new AuthResponse(accessToken, MapUser(user));
	}

	private static UserDto MapUser(ApplicationUser user)
		=> new(
			user.Id,
			user.Email ?? string.Empty,
			user.UserName ?? string.Empty,
			user.FirstName,
			user.LastName);
}