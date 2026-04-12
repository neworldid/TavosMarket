using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;

namespace TavosMarket.Client.Auth;

public sealed class TokenAuthenticationStateProvider(IJSRuntime jsRuntime) : AuthenticationStateProvider
{
	private const string TokenKey = "authToken";

	public override async Task<AuthenticationState> GetAuthenticationStateAsync()
	{
		var token = await jsRuntime.InvokeAsync<string?>("localStorage.getItem", TokenKey);

		if (string.IsNullOrWhiteSpace(token))
			return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

		var handler = new JwtSecurityTokenHandler();
		var jwt = handler.ReadJwtToken(token);

		var claims = jwt.Claims.ToList();
		var identity = new ClaimsIdentity(claims, "jwt");
		var user = new ClaimsPrincipal(identity);

		return new AuthenticationState(user);
	}

	public async Task SetTokenAsync(string token)
	{
		await jsRuntime.InvokeVoidAsync("localStorage.setItem", TokenKey, token);
		NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
	}

	public async Task ClearTokenAsync()
	{
		await jsRuntime.InvokeVoidAsync("localStorage.removeItem", TokenKey);
		NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
	}
}