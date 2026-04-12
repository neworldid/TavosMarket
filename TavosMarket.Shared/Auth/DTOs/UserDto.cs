namespace TavosMarket.Shared.Auth.DTOs;

public sealed record UserDto(
	Guid Id,
	string Email,
	string UserName,
	string? FirstName,
	string? LastName);