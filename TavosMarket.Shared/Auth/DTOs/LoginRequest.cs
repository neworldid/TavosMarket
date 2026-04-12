namespace TavosMarket.Shared.Auth.DTOs;

public sealed record LoginRequest(
	string Email,
	string Password);