using TavosMarket.Shared.Auth.DTOs;

namespace TavosMarket.Application.Auth;

public interface IAuthService
{
	Task<AuthResponse> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken);
	Task<AuthResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken);
	Task<UserDto> CurrentUserAsync(Guid userId, CancellationToken cancellationToken);
	
	Task<AuthResponse> GoogleLoginAsync(GoogleLoginRequest request, CancellationToken cancellationToken);
}