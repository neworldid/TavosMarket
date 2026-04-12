namespace TavosMarket.Shared.Auth.DTOs;

public sealed record RegisterRequest(
	string Email,
	string Password,
	string UserName,
	string? FirstName,
	string? LastName);