using TavosMarket.Shared.Auth.DTOs;

namespace TavosMarket.Application.Interfaces;

public interface IUserService
{
    Task<UserProfileDto?> GetProfileAsync(CancellationToken ct = default);
    Task<UserProfileDto> UpdateProfileAsync(UserProfileDto profileDto, CancellationToken ct = default);
    Task<int> GetTotalUsersCountAsync(CancellationToken ct = default);
}
