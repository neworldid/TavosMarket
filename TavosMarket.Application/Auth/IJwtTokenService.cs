using System.Security.Claims;

namespace TavosMarket.Application.Auth;

public interface IJwtTokenService
{
	string CreateAccessToken(ITokenUser user, IEnumerable<Claim>? extraClaims = null);
}