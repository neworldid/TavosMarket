using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using TavosMarket.Application.Auth;

namespace TavosMarket.Infrastructure.Auth;

public sealed class JwtTokenService(IConfiguration configuration) : IJwtTokenService
{
	public string CreateAccessToken(ITokenUser user, IEnumerable<Claim>? extraClaims = null)
	{
		var jwtSection = configuration.GetSection("Jwt");
		var key = jwtSection["Key"]!;
		var issuer = jwtSection["Issuer"]!;
		var audience = jwtSection["Audience"]!;
		var expiresMinutes = int.Parse(jwtSection["AccessTokenMinutes"]!);

		var claims = new List<Claim>
		{
			new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
			new(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
			new(ClaimTypes.NameIdentifier, user.Id.ToString()),
			new(ClaimTypes.Name, user.UserName ?? string.Empty),
			new(ClaimTypes.GivenName, user.FirstName ?? string.Empty),
			new(ClaimTypes.Surname, user.LastName ?? string.Empty)
		};

		if (extraClaims is not null)
			claims.AddRange(extraClaims);

		var credentials = new SigningCredentials(
			new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
			SecurityAlgorithms.HmacSha256);

		var token = new JwtSecurityToken(
			issuer: issuer,
			audience: audience,
			claims: claims,
			expires: DateTime.UtcNow.AddMinutes(expiresMinutes),
			signingCredentials: credentials);

		return new JwtSecurityTokenHandler().WriteToken(token);
	}
}