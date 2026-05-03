namespace TavosMarket.Application.Auth;

public interface ITokenUser
{
	Guid Id { get; }
	string? Email { get; }
	string? UserName { get; }
	string? FirstName { get; }
	string? LastName { get; }
}