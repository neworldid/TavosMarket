using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TavosMarket.Application.Auth;
using TavosMarket.Application.Interfaces;
using TavosMarket.Infrastructure.Identity;
using TavosMarket.Shared.Auth.DTOs;

namespace TavosMarket.Infrastructure.Auth;

public class UserService(
    UserManager<ApplicationUser> userManager,
    ICurrentUserService currentUserService) : IUserService
{
    public async Task<UserProfileDto?> GetProfileAsync(CancellationToken ct = default)
    {
        var userId = currentUserService.UserId;
        if (userId == null) return null;

        var user = await userManager.Users
            .FirstOrDefaultAsync(u => u.Id == userId, ct);

        if (user == null) return null;

        return MapToProfileDto(user);
    }

    public async Task<UserProfileDto> UpdateProfileAsync(UserProfileDto profileDto, CancellationToken ct = default)
    {
        var userId = currentUserService.UserId;
        if (userId == null || userId != profileDto.Id)
        {
            throw new UnauthorizedAccessException("You can only update your own profile.");
        }

        var user = await userManager.FindByIdAsync(userId.ToString() ?? string.Empty);
        if (user == null)
        {
            throw new InvalidOperationException("User not found.");
        }

        user.FirstName = profileDto.FirstName;
        user.LastName = profileDto.LastName;
        user.PhoneNumber = profileDto.PhoneNumber;

        var result = await userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            throw new InvalidOperationException(string.Join("; ", result.Errors.Select(e => e.Description)));
        }

        return MapToProfileDto(user);
    }

    public async Task<int> GetTotalUsersCountAsync(CancellationToken ct = default)
    {
        return await userManager.Users.CountAsync(ct);
    }

    private static UserProfileDto MapToProfileDto(ApplicationUser user)
    {
        return new UserProfileDto(
            user.Id,
            user.Email ?? string.Empty,
            user.UserName ?? string.Empty,
            user.FirstName,
            user.LastName,
            user.PhoneNumber);
    }
}
