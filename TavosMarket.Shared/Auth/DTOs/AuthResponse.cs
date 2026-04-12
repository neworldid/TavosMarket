namespace TavosMarket.Shared.Auth.DTOs;

public sealed record AuthResponse(
	string AccessToken,
	UserDto User);